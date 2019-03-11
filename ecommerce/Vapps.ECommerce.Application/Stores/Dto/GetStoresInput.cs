using Abp.Runtime.Validation;
using Vapps.Dto;
using Vapps.ECommerce.Orders;

namespace Vapps.ECommerce.Stores.Dto
{
    public class GetStoresInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 渠道来源(空-获取所有)
        /// </summary>
        public OrderSource? Source { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
