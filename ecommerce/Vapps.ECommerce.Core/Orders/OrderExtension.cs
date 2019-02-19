using System;
using Vapps.Extensions;
using Vapps.Helpers;

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
            order.OrderNumber = Guid.NewGuid().ToLongId().ToString();
        }

        /// <summary>
        /// 生成订单号（4位毫秒+2位随机数）
        /// </summary>
        /// <param name="order"></param>
        public static void GenerateOrderNumber(this Order order)
        {
            string sarNum = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var randomNum = CommonHelper.GenerateRandomDigitCode(2);

            order.OrderNumber = string.Concat(sarNum, randomNum);
        }
    }
}
