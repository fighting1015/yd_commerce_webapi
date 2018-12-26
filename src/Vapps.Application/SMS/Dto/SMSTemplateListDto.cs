using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

namespace Vapps.SMS.Dto
{
    [AutoMapFrom(typeof(SMSTemplate))]
    public class SMSTemplateListDto : EntityDto<long>, IPassivable, IHasCreationTime
    {
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
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}
