using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Dto.Shipments;

namespace Vapps.ECommerce.Orders.Dto
{
    public class OrderDetailDto : EntityDto<long>
    {
        public OrderDetailDto()
        {
            this.Items = new List<OrderDetailItemDto>();
            this.Shipments = new List<ShipmentDto>();
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 店铺
        /// </summary>
        public virtual string StoreId { get; set; }

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
        /// 收货地址(省份)
        /// </summary>
        public string ShippingProvice { get; set; }

        /// <summary>
        /// 收货地址(省份id)
        /// </summary>
        public int ShippingProviceId { get; set; }

        /// <summary>
        /// 收货地址(城市)
        /// </summary>
        public string ShippingCity { get; set; }

        /// <summary>
        /// 收货地址(城市Id)
        /// </summary>
        public int ShippingCityId { get; set; }

        /// <summary>
        /// 收货地址(区域/县)
        /// </summary>
        public string ShippingDistrict { get; set; }

        /// <summary>
        /// 收货地址(区域/县Id)
        /// </summary>
        public int ShippingDistrictId { get; set; }

        /// <summary>
        /// 收货地址(详细信息)
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// 收货地址(电话)
        /// </summary>
        public string ShippingPhoneNumber { get; set; }

        /// <summary>
        /// 收货地址(姓名)
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 付款时间(已付款状态才有值)
        /// </summary>
        public DateTime? PaidOn { get; set; }

        /// <summary>
        /// 签收时间(已签收状态才有值)
        /// </summary>
        public virtual DateTime? ReceivedOn { get; set; }

        /// <summary>
        /// 下单Ip地址
        /// </summary>
        public string IpAddress { get; set; }

        #region Order Amount

        /// <summary>
        /// 订单小计
        /// </summary>
        public decimal SubtotalAmount { get; set; }

        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 货运费用
        /// </summary>
        public decimal ShippingAmount { get; set; }

        /// <summary>
        /// 订单提成金额
        /// </summary>
        public decimal RewardAmount { get; set; }

        /// <summary>
        /// 支付方式附加费（货到付款手续费）
        /// </summary>
        public decimal PaymentMethodAdditionalFee { get; set; }

        /// <summary>
        /// 订单折扣（适用于订单总额）
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 订单折扣总额
        /// </summary>
        public decimal SubTotalDiscountAmount { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundedAmount { get; set; }

        #endregion

        /// <summary>
        /// 子订单
        /// </summary>
        public virtual List<OrderDetailItemDto> Items { get; set; }

        /// <summary>
        /// 发货记录
        /// </summary>
        public virtual List<ShipmentDto> Shipments { get; set; }
    }

    public class OrderDetailItemDto : EntityDto<long>
    {
        /// <summary>
        /// 子订单号
        /// </summary>
        public string OrderItemNumber { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 属性描述
        /// </summary>
        public virtual string AttributeDesciption { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 价格（小计）
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal UnitPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual decimal DiscountAmount { get; set; }

        /// <summary>
        /// 商品 / 属性图片
        /// </summary>
        public string PictureUrl { get; set; }
    }



}
