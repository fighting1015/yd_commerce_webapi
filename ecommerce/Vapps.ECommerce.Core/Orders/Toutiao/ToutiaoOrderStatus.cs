using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders.Toutiao
{
    public enum ToutiaoOrderStatus
    {
        /// <summary>
        /// 开始
        /// </summary>
        Start = 0,

        /// <summary>
        /// 待确认
        /// </summary>
        WaitForComfirm = 1,

        /// <summary>
        /// 备货中
        /// </summary>
        WaitForShip = 2,

        /// <summary>
        /// 已发货
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        Complated = 5,

        /// <summary>
        /// 退货
        /// </summary>
        Refunse = 6
    }


   
}
