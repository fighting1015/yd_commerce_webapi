using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Debugging;
using Vapps.Editions.Cache;
using Vapps.Editions.Dto;
using Vapps.Features;
using Vapps.MultiTenancy;
using Vapps.Payments;

namespace Vapps.Editions
{
    public class EditionSubscriptionAppService : VappsAppServiceBase, IEditionSubscriptionAppService
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IPaymentGatewayProviderFactory _paymentGatewayProviderFactory;
        private readonly ILocalizationContext _localizationContext;
        private readonly ISubscribableEditionCache _subscribableEditionCache;
        private readonly VappsTenantCache _tenantCache;
        private readonly EditionManager _editionManager;

        public EditionSubscriptionAppService(IMultiTenancyConfig multiTenancyConfig,
            IPaymentGatewayProviderFactory paymentGatewayProviderFactory,
            ILocalizationContext localizationContext,
            ISubscribableEditionCache subscribableEditionCache,
            VappsTenantCache tenantCache,
            EditionManager editionManager)
        {
            this._multiTenancyConfig = multiTenancyConfig;
            this._paymentGatewayProviderFactory = paymentGatewayProviderFactory;
            this._localizationContext = localizationContext;
            this._editionManager = editionManager;
            this._tenantCache = tenantCache;
            this._subscribableEditionCache = subscribableEditionCache;
        }

        #region Methods

        /// <summary>
        /// 获取版本展示信息
        /// </summary>
        /// <returns></returns>
        public async Task<EditionsSelectOutput> GetEditionsForSelect()
        {
            var features = FeatureManager
                .GetAll()
                .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);

            var flatFeatures = ObjectMapper
                .Map<List<FlatFeatureSelectDto>>(features)
                .OrderBy(f => f.DisplayName)
                .ToList();

            var editions = (await _editionManager.GetAllAsync())
                .Cast<SubscribableEdition>()
                .OrderBy(e => e.MonthlyPrice)
                .ToList();

            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);

            var editionWithFeatures = new List<EditionWithFeaturesForSelectDto>();
            foreach (var edition in editions)
            {
                editionWithFeatures.Add(await CreateEditionWithFeaturesDto(edition, featureDictionary));
            }

            int? tenantEditionId = null;
            if (AbpSession.UserId.HasValue)
            {
                tenantEditionId = (await TenantManager.GetByIdAsync(AbpSession.GetTenantId()))
                    .EditionId;
            }

            return new EditionsSelectOutput
            {
                AllFeatures = flatFeatures,
                EditionsWithFeatures = editionWithFeatures,
                TenantEditionId = tenantEditionId
            };
        }

        /// <summary>
        /// 获取当前账号版本信息
        /// </summary>
        /// <returns></returns>
        public async Task<EditionsViewOutput> GetCurrentEdition()
        {
            var features = FeatureManager
              .GetAll()
              .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);

            var flatFeatures = ObjectMapper
                .Map<List<FlatFeatureSelectDto>>(features)
                .OrderBy(f => f.DisplayName)
                .ToList();

            var tenant = _tenantCache.Get(AbpSession.GetTenantId());
            SubscribableEditionCacheItem edition;

            if (tenant.EditionId.HasValue)
                edition = await _subscribableEditionCache.GetAsync(tenant.EditionId.Value);
            else
                edition = await _subscribableEditionCache.GetDefaultAsync();

            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);
            return new EditionsViewOutput
            {
                AllFeatures = flatFeatures,
                EditionWithFeatures = await CreateEditionWithFeaturesDto(edition, featureDictionary),
            };
        }

        /// <summary>
        /// 获取版本详情
        /// </summary>
        /// <param name="editionId"></param>
        /// <returns></returns>
        public async Task<EditionSelectDto> GetEdition(int editionId)
        {
            var edition = await _editionManager.GetByIdAsync(editionId);
            var editionDto = ObjectMapper.Map<EditionSelectDto>(edition);
            foreach (var paymentGateway in Enum.GetValues(typeof(SubscriptionPaymentGatewayType)).Cast<SubscriptionPaymentGatewayType>())
            {
                var paymentGatewayManager = _paymentGatewayProviderFactory.Create(paymentGateway);
                var additionalData = await paymentGatewayManager.GetAdditionalPaymentData(ObjectMapper.Map<SubscribableEdition>(edition));
                editionDto.AdditionalData.Add(paymentGateway, additionalData);
            }

            return editionDto;
        }

        /// <summary>
        /// 版本试用
        /// </summary>
        /// <param name="editionId"></param>
        /// <returns></returns>
        public async Task TrialEdition(int editionId)
        {
            var edition = await _subscribableEditionCache.GetAsync(editionId);
            if (!edition.HasTrial())
                throw new UserFriendlyException(L("Edition.Trialed.CannotTrialed"));

            DateTime subscriptionEndDate = Clock.Now.AddDays(edition.TrialDayCount ?? 0);
            await TenantManager.TrialEditionAsync(
                tenantId: AbpSession.GetTenantId(),
                editionId: editionId,
                endDateUtc: subscriptionEndDate
            );
        }

        /// <summary>
        /// 升级到等价版本(价格相等)
        /// </summary>
        /// <param name="upgradeEditionId"></param>
        /// <returns></returns>
        public async Task UpgradeTenantToEquivalentEdition(int upgradeEditionId)
        {
            if (await UpgradeIsFree(upgradeEditionId))
            {
                await TenantManager.UpdateTenantAsync(

                    tenantId: AbpSession.GetTenantId(),
                    isActive: true,
                    isInTrialPeriod: false,
                    paymentPeriodType: null,
                    editionId: upgradeEditionId,
                    editionPaymentType: EditionPaymentType.Upgrade
                );
            }
        }

        #endregion

        #region Utilities

        private async Task<bool> UpgradeIsFree(int upgradeEditionId)
        {
            var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (!tenant.EditionId.HasValue)
            {
                throw new Exception("Tenant must be assigned to an Edition in order to upgrade !");
            }

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(upgradeEditionId);
            var bothEditionsAreFree = targetEdition.IsFree && currentEdition.IsFree;
            var bothEditionsHasSamePrice = currentEdition.HasSamePrice(targetEdition);
            return bothEditionsAreFree || bothEditionsHasSamePrice;
        }

        private async Task<EditionWithFeaturesForSelectDto> CreateEditionWithFeaturesDto(SubscribableEdition edition, Dictionary<string, Feature> featureDictionary)
        {
            var editionWithFeatures = new EditionWithFeaturesForSelectDto
            {
                Edition = ObjectMapper.Map<EditionSelectDto>(edition),
                FeatureValues = await GetFeatureValues(edition, featureDictionary)
            };

            foreach (var paymentGateway in Enum.GetValues(typeof(SubscriptionPaymentGatewayType)).Cast<SubscriptionPaymentGatewayType>())
            {
                var paymentGatewayManager = _paymentGatewayProviderFactory.Create(paymentGateway);
                var additionalData = await paymentGatewayManager.GetAdditionalPaymentData(edition);
                editionWithFeatures.Edition.AdditionalData.Add(paymentGateway, additionalData);
            }

            return editionWithFeatures;
        }

        private async Task<List<FeatureValueDto>> GetFeatureValues(SubscribableEdition edition, Dictionary<string, Feature> featureDictionary)
        {
            var result = new List<FeatureValueDto>();

            var featureValues = (await _editionManager.GetFeatureNestValuesAsync(edition.Id))
                                .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name));

            foreach (var item in featureValues)
            {
                var itemDto = new FeatureValueDto()
                {
                    Name = item.Name,
                    Value = featureDictionary[item.Name].GetValueText(item.Value, _localizationContext)
                };

                if (item.Childs != null)
                {
                    itemDto.Childs = new List<NameValueDto>();
                    foreach (var childItem in item.Childs)
                    {
                        itemDto.Childs.Add(new NameValueDto(
                                    childItem.Name,
                                    featureDictionary[childItem.Name].GetValueText(childItem.Value, _localizationContext))
                                );
                    }
                }
                result.Add(itemDto);
            }

            return result;
        }

        private async Task<EditionWithFeaturesDto> CreateEditionWithFeaturesDto(SubscribableEditionCacheItem edition, Dictionary<string, Feature> featureDictionary)
        {
            return new EditionWithFeaturesDto
            {
                Edition = ObjectMapper.Map<EditionSelectDto>(edition),
                FeatureValues = (await _editionManager.GetFeatureValuesAsync(edition.Id))
                    .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name))
                    .Select(fv => new NameValueDto(
                        fv.Name,
                        featureDictionary[fv.Name].GetValueText(fv.Value, _localizationContext))
                    )
                    .ToList()
            };
        }

        private void CheckTenantRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfTenantRegistrationIsDisabledMessage_Detail"));
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                throw new UserFriendlyException(L("MultiTenancyIsNotEnabled"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration);
        }

        #endregion
    }
}
