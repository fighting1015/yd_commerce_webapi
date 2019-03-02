using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class CreateOrUpdateShipmentInput : EntityDto<long>
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string TrackingNumber { get; set; }

        /// <summary>
        /// 快递Id
        /// </summary>
        public virtual string LogisticsId { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public virtual ShippingStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string AdminComment { get; set; }

        /// <summary>
        /// 重量(如果有)
        /// </summary>
        public virtual decimal TotalWeight { get; set; }

        /// <summary>
        /// 体积(如果有)
        /// </summary>
        public virtual decimal TotalVolume { get; set; }

        /// <summary>
        /// 发货条目
        /// </summary>
        public virtual List<ShipmentItemDto> Items { get; set; }
    }
}
