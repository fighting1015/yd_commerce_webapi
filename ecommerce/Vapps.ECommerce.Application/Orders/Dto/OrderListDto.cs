using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Dto
{
    public class OrderListDto : EntityDto<long>
    {
        public OrderListDto()
        {
            this.Items = new List<OrderListItemDto>();
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 店铺
        /// </summary>
        public virtual string Store { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual string OrderStatusString { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public virtual string PaymentStatusString { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public virtual PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public virtual string ShippingStatusString { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public virtual ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public virtual string OrderTypeString { get; set; }

        /// <summary>
        /// 订单类型Id
        /// </summary>
        public virtual OrderType OrderType { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public virtual string OrderSourceString { get; set; }

        /// <summary>
        /// 订单来源Id
        /// </summary>
        public virtual OrderSource OrderSource { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public virtual string AdminComment { get; set; }

        /// <summary>
        /// 客户备注
        /// </summary>
        public virtual string CustomerComment { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public virtual decimal TotalAmount { get; set; }

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
        /// 子订单
        /// </summary>
        public virtual List<OrderListItemDto> Items { get; set; }
    }

    public class OrderListItemDto : EntityDto<long>
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 属性描述
        /// </summary>
        public string AttributeDesciption { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 价格（小计）
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal UnitPrice { get; set; }

        /// <summary>
        /// 商品 / 属性图片
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
