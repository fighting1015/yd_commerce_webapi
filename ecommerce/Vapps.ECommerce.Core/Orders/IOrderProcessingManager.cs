using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders
{
    public interface IOrderProcessingManager
    {
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="notifyCustomer">是否通知客户</param>
        Task Ship(Shipment shipment, bool notifyCustomer);

        /// <summary>
        /// 检查订单状态
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Validated order</returns>
        Task CheckOrderStatus(Order order);

        /// <summary>
        /// 设置订单状态
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="os">目标状态</param>
        /// <param name="notifyCustomer">是否通知客户</param>
        Task SetOrderStatus(Order order, OrderStatus os, bool notifyCustomer);
    }
}
