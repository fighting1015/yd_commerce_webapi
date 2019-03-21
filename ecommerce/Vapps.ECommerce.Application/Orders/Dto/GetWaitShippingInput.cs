using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Dto;

namespace Vapps.ECommerce.Orders.Dto
{
    public class GetWaitShippingInput
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public List<long> ProductIds { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public List<long> StoreIds { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 下单时间(utc)
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
    }
}
