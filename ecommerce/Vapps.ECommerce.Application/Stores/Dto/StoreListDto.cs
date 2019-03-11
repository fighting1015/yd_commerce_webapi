using Abp.Application.Services.Dto;
using Vapps.ECommerce.Orders;

namespace Vapps.ECommerce.Stores.Dto
{
    //[AutoMapFrom(typeof(Store))]
    public class StoreListDto : EntityDto
    {
        /// <summary>
        /// 店铺名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource OrderSourceType { get; set; }

        /// <summary>
        /// 订单同步
        /// </summary>
        public bool OrderSync { get; set; }
    }
}
