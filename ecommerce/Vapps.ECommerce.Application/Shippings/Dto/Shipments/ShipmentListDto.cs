using Abp.Application.Services.Dto;
using System;

namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class ShipmentListDto : EntityDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string TrackingNumber { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string LogisticsName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public virtual DateTime? DeliveryOd { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public virtual DateTime? ReceivedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string AdminComment { get; set; }
    }
}
