using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.ECommerce.Customers.Dto
{
    public class GetCustomersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 消费金额（起始）
        /// </summary>
        public decimal ConsumesForm { get; set; }

        /// <summary>
        /// 消费金额（结束）
        /// </summary>
        public decimal ConsumesTo { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }

    }
}
