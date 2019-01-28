using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders
{

    public static class OrderExtension
    {
        /// <summary>
        /// 重置订单Guid和Number
        /// </summary>
        /// <param name="order"></param>
        public static void ResetOrderGuidAndNumber(this Order order)
        {
            order.OrderGuid = Guid.NewGuid();
            order.CustomOrderNumber = order.OrderGuid.ToLongId().ToString();
        }
    }
}
