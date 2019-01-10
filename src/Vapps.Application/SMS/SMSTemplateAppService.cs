using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Caching;
using Vapps.Dto;
using Vapps.SMS.Dto;

namespace Vapps.SMS
{
    [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Self)]
    public class SMSTemplateAppService : VappsAppServiceBase, ISMSTemplateAppService
    {
        private readonly ISMSTemplateManager _smsTemplateManager;
        private readonly ISMSConfiguration _smsConfiguration;
        private readonly ICacheManager _cacheManager;

        public SMSTemplateAppService(ISMSTemplateManager smsTemplateManager,
            ISMSConfiguration smsConfiguration,
            ICacheManager cacheManager)
        {
            this._smsTemplateManager = smsTemplateManager;
            this._smsConfiguration = smsConfiguration;
            this._cacheManager = cacheManager;
        }

        #region Method

        /// <summary>
        /// 获取所有短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SMSTemplateListDto>> GetSMSTemplates(GetSMSTemplatesInput input)
        {
            var query = _smsTemplateManager
                .SMSTemplates
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name))
                .WhereIf(!input.TemplateCode.IsNullOrWhiteSpace(), r => r.TemplateCode.Contains(input.TemplateCode))
                .WhereIf(!input.ProviderName.IsNullOrWhiteSpace(), r => r.SmsProvider.Contains(input.ProviderName))
                .WhereIf(input.IsActived.HasValue, r => r.IsActive == input.IsActived.Value);

            var tempalateCount = await query.CountAsync();

            var tempalates = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var templateListDtos = ObjectMapper.Map<List<SMSTemplateListDto>>(tempalates);
            return new PagedResultDto<SMSTemplateListDto>(
                tempalateCount,
                templateListDtos);
        }

        /// <summary>
        /// 获取所有可用短信模板(下拉框)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItemDto>> GetAvailableSMSTemplates()
        {
            return await _cacheManager.GetSelectListItemCache().GetAsync(ApplicationCacheNames.AvailableSmsTemplate, async () =>
            {
                var query = _smsTemplateManager.SMSTemplates.Where(st => st.IsActive);

                var tempalateCount = await query.CountAsync();
                var tempalates = await query
                    .OrderByDescending(st => st.Id)
                    .ToListAsync();

                var tempalteSelectListItem = tempalates.Select(x =>
                {
                    return new SelectListItemDto
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return tempalteSelectListItem;
            });
        }

        /// <summary>
        /// 获取短信模板详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Create, AdminPermissions.ContentManage.SmsTemplates.Edit)]
        public async Task<GetSMSTemplateForEditDto> GetSMSTemplateForEdit(NullableIdDto<long> input)
        {
            GetSMSTemplateForEditDto templateEditDto;
            if (input.Id.HasValue) //Editing existing template?
            {
                var template = await _smsTemplateManager.GetByIdAsync(input.Id.Value);
                templateEditDto = ObjectMapper.Map<GetSMSTemplateForEditDto>(template);

                await _smsTemplateManager.SMSTemplateRepository.EnsureCollectionLoadedAsync(template, t => t.Items);
                templateEditDto.Items = template.Items.Select(x => ObjectMapper.Map<SMSTemplateItemDto>(x)).ToList();
            }
            else
            {
                templateEditDto = new GetSMSTemplateForEditDto();
            }

            templateEditDto.AvailabelSmsProviders = _smsConfiguration.Providers.Select(x => new SMSProviderInfoDto()
            {
                DisplayName = L(string.Format(VappsConsts.SMSProviderLocalizationName, x.Name)),
                SystemName = x.Name,
            }).ToList();

            return templateEditDto;
        }

        /// <summary>
        /// 新建/更新短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Create, AdminPermissions.ContentManage.SmsTemplates.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateSMSTemplateInput input)
        {
            CheckSMSProvider(input.SmsProvider);
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                await UpdateTemplateAsync(input);
            }
            else
            {
                await CreateTemplateAsync(input);
            }
        }

        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Delete)]
        public async Task DeleteAsync(EntityDto<long> input)
        {
            var template = await _smsTemplateManager.GetByIdAsync(input.Id);
            await _smsTemplateManager.DeleteAsync(template);
        }

        /// <summary>
        /// 获取可用短信供应商
        /// </summary>
        /// <returns></returns>
        public List<SMSProviderInfoDto> GetSMSProviders()
        {
            var providers = _smsConfiguration.Providers.Select(x => new SMSProviderInfoDto()
            {
                DisplayName = L(string.Format(VappsConsts.SMSProviderLocalizationName, x.Name)),
                SystemName = x.Name
            }).ToList();

            return providers;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 创建短信模板
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Create)]
        protected virtual async Task CreateTemplateAsync(CreateOrUpdateSMSTemplateInput input)
        {
            var template = new SMSTemplate()
            {
                Name = input.Name,
                TemplateCode = input.TemplateCode,
                SmsProvider = input.SmsProvider,
                IsActive = input.IsActive,
                Items = input.Items.Select(x => new SMSTemplateItem()
                {
                    DataItemName = x.DataItemName,
                    DataItemValue = x.DataItemValue
                }).ToList(),
            };

            await _smsTemplateManager.CreateAsync(template);
        }

        /// <summary>
        /// 更新短信模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.ContentManage.SmsTemplates.Edit)]
        protected virtual async Task UpdateTemplateAsync(CreateOrUpdateSMSTemplateInput input)
        {
            Debug.Assert(input.Id != null, "input.Edition.Id should be set.");

            var template = await _smsTemplateManager.GetByIdAsync(input.Id.Value);
            template.Name = input.Name;
            template.TemplateCode = input.TemplateCode;
            template.SmsProvider = input.SmsProvider;
            template.IsActive = input.IsActive;

            await _smsTemplateManager.SMSTemplateRepository.EnsureCollectionLoadedAsync(template, t => t.Items);

            //删除不存在的参数
            var existItemIds = input.Items.Select(i => i.Id);
            var items2Remove = template.Items.Where(i => !existItemIds.Contains(i.Id)).ToList();
            foreach (var item in items2Remove)
            {
                template.Items.Remove(item);
            }

            //添加或更新参数
            foreach (var itemInput in input.Items)
            {
                if (itemInput.Id.HasValue && itemInput.Id.Value > 0)
                {
                    var item = template.Items.FirstOrDefault(x => x.Id == itemInput.Id.Value);
                    if (item != null)
                    {
                        item.DataItemName = itemInput.DataItemName;
                        item.DataItemValue = itemInput.DataItemValue;
                    }
                }
                else
                {
                    template.Items.Add(new SMSTemplateItem()
                    {
                        DataItemName = itemInput.DataItemName,
                        DataItemValue = itemInput.DataItemValue,
                    });
                }
            }

            await _smsTemplateManager.UpdateAsync(template);
        }

        /// <summary>
        /// 检查短信供应商是否有效
        /// </summary>
        /// <param name="providerName"></param>
        private void CheckSMSProvider(string providerName)
        {
            var providers = _smsConfiguration.Providers;
            if (providers.FirstOrDefault(p => p.Name == providerName) == null)
                throw new UserFriendlyException("SmsTemplate.Validator.InvalidSMSProvider");
        }
    }

    #endregion

}
