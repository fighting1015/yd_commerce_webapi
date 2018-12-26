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
        /// �⻧����
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(Tenant.TenancyNameRegex)]
        public string TenancyName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// ����Ա�����ַ
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        /// <summary>
        /// ����Ա�ֻ�����
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// ����Ա����
        /// </summary>
        [StringLength(AbpUserBase.MaxPasswordLength)]
        public string AdminPassword { get; set; }

        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>
        [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// �´ε�¼��Ҫ�޸�����
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }

        /// <summary>
        /// ���ͼ����ʼ�
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        /// �汾Id
        /// </summary>
        public int? EditionId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public bool IsActive { get; set; }

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }
    }
}