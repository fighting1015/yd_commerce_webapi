using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 商店id
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// 用户id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// 货运地址id
        /// </summary>
        public int? ShippingAddressId { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethodSystemName { get; set; }

        /// <summary>
        /// 付款银行名称
        /// </summary>
        public string PaymentBankSystemName { get; set; }

        /// <summary>
        /// 订单小计
        /// </summary>
        public decimal OrderSubtotal { get; set; }

        /// <summary>
        /// 货运费用
        /// </summary>
        public decimal OrderShippingFree { get; set; }

        /// <summary>
        /// 订单提成金额
        /// </summary>
        public decimal DistributorReward { get; set; }

        /// <summary>
        /// 支付方式附加费
        /// </summary>
        public decimal PaymentMethodAdditionalFee { get; set; }

        /// <summary>
        /// 订单折扣（适用于订单总额）
        /// </summary>
        public decimal OrderDiscount { get; set; }

        /// <summary>
        /// 订单折扣总额
        /// </summary>
        public decimal OrderSubTotalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the order total
        /// 订单总额
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the refunded amount
        /// 退款金额
        /// </summary>
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// 用户Ip地址
        /// </summary>
        public string CustomerIp { get; set; }

        /// <summary>
        /// 交易订单号
        /// </summary>
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PaidDateUtc { get; set; }

        /// <summary>
        /// 货运方式
        /// </summary>
        public string ShippingMethod { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order creation
        /// 创建时间
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 订单号 GUID转成的19位字符串
        /// </summary>
        public string CustomOrderNumber { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string CustomerComment { get; set; }

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


        #endregion Custom properties
    }
}
