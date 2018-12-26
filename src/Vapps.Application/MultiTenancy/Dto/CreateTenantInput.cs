using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Vapps.Authorization.Users;
using System;

namespace Vapps.MultiTenancy.Dto
{
    public class CreateTenantInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(Tenant.TenancyNameRegex)]
        public string TenancyName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 管理员邮箱地址
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        /// <summary>
        /// 管理员手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [StringLength(AbpUserBase.MaxPasswordLength)]
        public string AdminPassword { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 下次登录需要修改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// 发送激活邮件
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public int? EditionId { get; set; }

        /// <summary>
        /// 激活
        /// </summary>
        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }
    }
}