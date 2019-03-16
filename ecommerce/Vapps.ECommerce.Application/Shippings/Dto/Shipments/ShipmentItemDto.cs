using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class ShipmentItemDto : EntityDto<long>
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public string AttributeInfo { get; set; }

        /// <summary>
        /// 订单item id
        /// </summary>
        public long OrderItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
