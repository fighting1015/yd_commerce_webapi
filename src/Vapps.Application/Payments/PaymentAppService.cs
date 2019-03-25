using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Editions;
using Vapps.Editions.Cache;
using Vapps.Editions.Dto;
using Vapps.MultiTenancy;
using Vapps.MultiTenancy.Dto;
using Vapps.Payments.Cache;
using Vapps.Payments.Dto;
using Vappse.MultiTenancy.Payments.Dto;

namespace Vapps.Payments
{
    public class PaymentAppService : VappsAppServiceBase, IPaymentAppService
    {
        private readonly ISubscriptionPaymentManager _subscriptionPaymentManager;
        private readonly IPaymentGatewayProviderFactory _paymentGatewayProviderFactory;
        private readonly ISubscriptionPaymentCache _subscriptionPaymentCache;
        private readonly IPaymentIdCache _paymentIdCache;
        private readonly ISubscribableEditionCache _editionCache;
        private readonly EditionManager _editionManager;

        public PaymentAppService(ISubscriptionPaymentManager subscriptionPaymentManager,
            IPaymentGatewayProviderFactory paymentGatewayProviderFactory,
           ISubscriptionPaymentCache subscriptionPaymentCache,
            IPaymentIdCache paymentIdCache,
            ISubscribableEditionCache editionCache,
            EditionManager editionManager)
        {
            _subscriptionPaymentManager = subscriptionPaymentManager;
            _editionManager = editionManager;
            _paymentGatewayProviderFactory = paymentGatewayProviderFactory;
            _subscriptionPaymentCache = subscriptionPaymentCache;
            _editionCache = editionCache;
            _paymentIdCache = paymentIdCache;
        }

        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input)
        {
            var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("TenantEditionIsNotAssigned"));
            }

            var currentEdition = await _editionCache.GetAsync(tenant.EditionId.Value);
            var targetEdition = input.UpgradeEditionId == null ? currentEdition : await _editionCache.GetAsync(input.UpgradeEditionId.Value);

            decimal additionalPrice = 0;
            if (input.UpgradeEditionId.HasValue)
            {
                var remainingDaysCount = tenant.CalculateRemainingDayCount();
                if (remainingDaysCount > 0)
                {
                    additionalPrice = _subscriptionPaymentManager.GetUpgradePrice(
                            currentEdition,
                            targetEdition,
                            remainingDaysCount
                        );
                }
            }

            var edition = ObjectMapper.Map<EditionSelectDto>(input.UpgradeEditionId == null ? currentEdition : targetEdition);
            await SetAdditionalDataForPaymentGateways(edition);

            return new PaymentInfoDto
            {
                Edition = edition,
                AdditionalPrice = additionalPrice
            };
        }

        /// <summary>
        /// 创建支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentDto input)
        {
            return await UnifiedCreatePayment(input);
        }

        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CancelPayment(CancelPaymentDto input)
        {
            await Task.FromResult(0);
        }

        /// <summary>
        /// 创建Js支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreatePaymentResponse> CreateJsPayment(CreatePaymentDto input)
        {
            return await UnifiedCreatePayment(input, true);
        }

        private async Task<CreatePaymentResponse> UnifiedCreatePayment(CreatePaymentDto input, bool jsPayment = false)
        {
            var paymentId = _paymentIdCache.GetCacheItem(AbpSession.GetTenantId(),
                            input.EditionId,
                            input.EditionPaymentType,
                            input.PaymentPeriodType);

            var targetEdition = await _editionCache.GetAsync(input.EditionId);
            var tenant = AbpSession.TenantId == null ? null : await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            var amount = await CalculateAmountForPaymentAsync(targetEdition, input.PaymentPeriodType, input.EditionPaymentType, tenant);

            var paymentGatewayManager = _paymentGatewayProviderFactory.Create(input.SubscriptionPaymentGatewayType);

            CreatePaymentResponse createPaymentResult;
            if (jsPayment)
            {
                createPaymentResult = await paymentGatewayManager.CreateJsPaymentAsync(new CreatePaymentRequest()
                {
                    PaymentId = paymentId,
                    Description = GetPaymentDescription(input, targetEdition),
                    Amount = amount,
                    UserId = AbpSession.GetUserId()
                });
            }
            else
            {
                createPaymentResult = await paymentGatewayManager.CreatePaymentAsync(new CreatePaymentRequest()
                {
                    PaymentId = paymentId,
                    Description = GetPaymentDescription(input, targetEdition),
                    Amount = amount
                });
            }

            var cacheItem = await _subscriptionPaymentCache.GetCacheItemOrNullAsync(paymentId);
            if (cacheItem == null)
            {
                await _subscriptionPaymentCache.AddCacheItemAsync(
                      new SubscriptionPaymentCacheItem
                      {
                          PaymentPeriodType = input.PaymentPeriodType,
                          EditionId = input.EditionId,
                          TenantId = tenant == null ? 0 : tenant.Id,
                          Amount = amount,
                          DayCount = input.PaymentPeriodType.HasValue ? (int)input.PaymentPeriodType.Value : 0,
                          PaymentId = paymentId,
                          Status = SubscriptionPaymentStatus.Processing,
                          EditionPaymentType = input.EditionPaymentType
                      });
            }

            createPaymentResult.Amount = amount;
            createPaymentResult.PaymentId = paymentId;
            return createPaymentResult;
        }

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<QueryPaymentOutput> QueryPayment(string paymentId)
        {
            var result = new QueryPaymentOutput();
            var paymentCache = await _subscriptionPaymentCache.GetCacheItemOrNullAsync(paymentId);

            if (paymentCache != null && paymentCache.Status == SubscriptionPaymentStatus.Completed)
                result.Paid = true;

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取支付记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input)
        {
            var query = _subscriptionPaymentManager.SubscriptionPayments
                .Include(sp => sp.Edition)
                .Where(sp => sp.TenantId == AbpSession.GetTenantId())
                .OrderBy(input.Sorting);

            var paymentsCount = await query.CountAsync();
            var payments = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            List<SubscriptionPaymentListDto> paymentDtoList = new List<SubscriptionPaymentListDto>();

            foreach (var payment in payments)
            {
                var paymentDtoItem = ObjectMapper.Map<SubscriptionPaymentListDto>(payment);
                paymentDtoItem.Gateway = payment.Gateway.GetLocalizedEnum(LocalizationManager);
                paymentDtoItem.PaymentPeriodType = payment.PaymentPeriodType.HasValue ? payment.PaymentPeriodType.Value.GetLocalizedEnum(LocalizationManager) : string.Empty;
                paymentDtoItem.Status = payment.Status.GetLocalizedEnum(LocalizationManager);
                paymentDtoList.Add(paymentDtoItem);
            }

            return new PagedResultDto<SubscriptionPaymentListDto>(paymentsCount, paymentDtoList);
        }

        private async Task SetAdditionalDataForPaymentGateways(EditionSelectDto edition)
        {
            foreach (var paymentGateway in Enum.GetValues(typeof(SubscriptionPaymentGatewayType)).Cast<SubscriptionPaymentGatewayType>())
            {
                var paymentGatewayProvider = _paymentGatewayProviderFactory.Create(paymentGateway);

                if (!paymentGatewayProvider.IsEnable)
                    continue;

                var additionalData = await paymentGatewayProvider.GetAdditionalPaymentData(ObjectMapper.Map<SubscribableEdition>(edition));
                edition.AdditionalData.Add(paymentGateway, additionalData);
            }
        }

        private async Task<decimal> CalculateAmountForPaymentAsync(SubscribableEditionCacheItem targetEdition, PaymentPeriodType? periodType, EditionPaymentType editionPaymentType, Tenant tenant)
        {
            if (editionPaymentType != EditionPaymentType.Upgrade)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("CanNotUpgradeSubscriptionSinceTenantHasNoEditionAssigned"));
            }

            var remainingDaysCount = tenant.CalculateRemainingDayCount();

            if (remainingDaysCount <= 0)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            Debug.Assert(tenant.EditionId != null, "tenant.EditionId != null");

            var currentEdition = await _editionCache.GetAsync(tenant.EditionId.Value);

            return _subscriptionPaymentManager.GetUpgradePrice(currentEdition, targetEdition, remainingDaysCount);
        }

        private string GetPaymentDescription(CreatePaymentDto input, SubscribableEditionCacheItem targetEdition)
        {
            switch (input.EditionPaymentType)
            {
                case EditionPaymentType.NewRegistration:
                case EditionPaymentType.BuyNow:
                    return L("Edition.Purchase");
                case EditionPaymentType.Upgrade:
                    return L("Edition.UpgradedTo", targetEdition.DisplayName);
                case EditionPaymentType.Extend:
                    return L("Edition.Extended", targetEdition.DisplayName);
                default:
                    throw new ArgumentException(nameof(input.EditionPaymentType));
            }
        }


    }
}
