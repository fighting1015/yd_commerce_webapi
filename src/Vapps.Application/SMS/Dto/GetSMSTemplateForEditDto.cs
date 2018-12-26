using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Vapps.SMS.Dto
{
    [AutoMapFrom(typeof(SMSTemplate))]
    public class GetSMSTemplateForEditDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
        public GetSMSTemplateForEditDto()
        {
            Items = new List<SMSTemplateItemDto>();
        }

        /// <summary>
        /// 模板消息名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短息模板编号（第三方短信编号）
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短信供应商名称
        /// </summary>
        public string SmsProvider { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 模板参数集合
        /// </summary>
        public List<SMSTemplateItemDto> Items { get; set; }

        /// <summary>
        /// 可用的短信供应商
        /// </summary>
        public List<SMSProviderInfoDto> AvailabelSmsProviders { get; set; }
    }

    [AutoMapFrom(typeof(SMSTemplateItem))]
    public class SMSTemplateItemDto : SMSTemplateItemInput
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
