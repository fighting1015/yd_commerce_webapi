using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Products.Dto;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Dto
{
    public class CreateOrUpdateOrderInput : EntityDto
    {
        /// <summary>
        /// 订单号（为空则自动生成）
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public virtual string StoreId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public virtual PaymentStatus PaymentStatus { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public virtual ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public virtual OrderType OrderType { get; set; }

        /// <summary>
        /// 订单来源
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

        #region Address Info

        /// <summary>
        /// 收货地址(省份id)
        /// </summary>
        public int ShippingProviceId { get; set; }

        /// <summary>
        /// 收货地址(城市Id)
        /// </summary>
        public int ShippingCityId { get; set; }

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
        /// 订单提成/佣金
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
        /// 子订单(商品)
        /// </summary>
        public virtual List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto : EntityDto<long>
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
        /// 数量
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal UnitPrice { get; set; }

        /// <summary>
        /// 价格（小计）
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual decimal DiscountAmount { get; set; }

        /// <summary>
        /// 属性值（如果有）
        /// </summary>
        public List<ProductAttributeDto> Attributes { get; set; }
    }
}
