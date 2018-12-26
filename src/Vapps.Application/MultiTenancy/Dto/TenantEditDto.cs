using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;
using System;

namespace Vapps.MultiTenancy.Dto
{
    [AutoMap(typeof(Tenant))]
    public class TenantEditDto : EntityDto
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public int? EditionId { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 订阅结束时间
        /// </summary>
        public DateTime? SubscriptionEndDateUtc { get; set; }

        /// <summary>
        /// 是否试用中
        /// </summary>
        public bool IsInTrialPeriod { get; set; }

        /// <summary>
        /// 已有(设置的)特性值,更新时要赋值
        /// </summary>
        public List<NameValueDto> Features { get; set; }
    }
}