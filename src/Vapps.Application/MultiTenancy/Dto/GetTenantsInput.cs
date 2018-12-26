using Abp.Runtime.Validation;
using System;
using Vapps.Dto;

namespace Vapps.MultiTenancy.Dto
{
    public class GetTenantsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 租户系统名称
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 订阅(购买)时间-开始
        /// </summary>
        public DateTime? SubscriptionEndDateStart { get; set; }

        /// <summary>
        /// 订阅(购买)时间-结束
        /// </summary>
        public DateTime? SubscriptionEndDateEnd { get; set; }

        /// <summary>
        /// 创建时间-开始
        /// </summary>
        public DateTime? CreationDateStart { get; set; }

        /// <summary>
        /// 创建时间-结束
        /// </summary>
        public DateTime? CreationDateEnd { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int? EditionId { get; set; }

        /// <summary>
        /// 指定版本
        /// </summary>
        public bool EditionIdSpecified { get; set; }

        /// <summary>
        /// 是否启用(空代表全部)
        /// </summary>
        public bool? IsActive { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }

            Sorting = Sorting.Replace("editionDisplayName", "Edition.DisplayName");
        }
    }
}

