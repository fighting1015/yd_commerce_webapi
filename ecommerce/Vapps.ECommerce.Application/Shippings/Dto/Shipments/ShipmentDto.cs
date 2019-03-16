using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class ShipmentDto : EntityDto
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string LogisticsNumber { get; set; }

        /// <summary>
        /// 快递Id
        /// </summary>
        public virtual int LogisticsId { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string LogisticsName { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public virtual ShippingStatus Status { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public virtual string StatusString { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public virtual DateTime DeliveryOn { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public virtual DateTime? ReceivedOn { get; set; }

        /// <summary>
        /// 收货姓名
        /// </summary>
        public virtual string ShippingName { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        public virtual string ShippingPhoneNumber { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public virtual string ShippingAddress { get; set; }

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
