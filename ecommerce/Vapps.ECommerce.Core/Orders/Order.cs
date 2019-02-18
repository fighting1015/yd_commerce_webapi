using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shipping;

namespace Vapps.ECommerce.Orders
{
    /// <summary>
    /// Represents an order
    /// </summary>
    public partial class Order : FullAuditedEntity<long>, IMustHaveTenant
    {
        private ICollection<OrderItem> _orderItems;
        private ICollection<Shipment> _shipments;

        #region Properties

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 商店id
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        #region Shipping Address

        /// <summary>
        /// 收货地址(省份)
        /// </summary>
        public string ShippingProvice { get; set; }

        /// <summary>
        /// 收货地址(城市)
        /// </summary>
        public string ShippingCity { get; set; }

        /// <summary>
        /// 收货地址(区域/县)
        /// </summary>
        public string ShippingDistrict { get; set; }

        /// <summary>
        /// 收货地址(详细信息)
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// 收货地址(电话)
        /// </summary>
        public string ShippingTepephone { get; set; }

        /// <summary>
        /// 收货地址(姓名)
        /// </summary>
        public string ShippingName { get; set; }

        #endregion

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
        /// 支付方式附加费
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
        /// Gets or sets the refunded amount
        /// 退款金额
        /// </summary>
        public decimal RefundedAmount { get; set; }

        #endregion

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethodSystemName { get; set; }

        /// <summary>
        /// 付款银行名称
        /// </summary>
        public string PaymentBankSystemName { get; set; }

        /// <summary>
        /// 交易订单号
        /// </summary>
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// 用户Ip地址
        /// </summary>
        public string CustomerIp { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }

        /// <summary>
        /// 货运方式
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string CustomerComment { get; set; }

        #endregion Properties

        #region Enum Properties

        /// <summary>
        /// Gets or sets the order status
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// Gets or sets the Order source type
        /// </summary>
        public OrderSource OrderSource { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets order items
        /// 订单条目
        /// </summary>
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Gets or sets shipments
        /// 发货条目
        /// </summary>
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
