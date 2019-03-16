using System;
using System.Collections.Generic;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Dto
{
    public class GetOrderForEditOutput
    {
        public GetOrderForEditOutput()
        {
            this.Items = new List<OrderItemDto>();
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 店铺
        /// </summary>
        public virtual int StoreId { get; set; }

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
        /// 订单类型Id
        /// </summary>
        public virtual OrderType OrderType { get; set; }

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
        /// 收货地址(省份id)
        /// </summary>
        public int ShippingProvinceId { get; set; }

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
        public virtual List<OrderItemDto> Items { get; set; }
    }
}
