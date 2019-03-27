using Abp.Runtime.Validation;
using System.Collections.Generic;
using Vapps.Dto;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Dto
{
    public class GetOrdersInput : GetOrdersBaseInput
    {
        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string LogisticsNumber { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public DateRangeDto ReceivedOn { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual OrderStatus[] OrderStatuses { get; set; }

        /// <summary>
        /// 付款状态
        /// </summary>
        public virtual PaymentStatus[] PaymentStatuses { get; set; }

        /// <summary>
        /// 发货状态
        /// </summary>
        public virtual ShippingStatus[] ShippingStatuses { get; set; }
    }

    public class GetOrdersBaseInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 店铺Id
        /// </summary>
        public List<long> StoreIds { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public List<long> ProductIds { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateRangeDto CreatedOn { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 省份Id
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int? DistrictId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public virtual OrderType[] OrderTypes { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public virtual OrderSource[] OrderSources { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public virtual string AdminComment { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public virtual string CustomerComment { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime,Id DESC";
            }
        }
    }
}
