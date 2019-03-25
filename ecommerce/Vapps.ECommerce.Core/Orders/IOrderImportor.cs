using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders
{
    public interface IOrderImportor
    {
        /// <summary>
        /// 导入订单
        /// </summary>
        /// <param name="orderImport"></param>
        /// <returns></returns>
        Task<bool> ImportOrderAsync(OrderImport orderImport);

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="orderImport"></param>
        /// <param name="order"></param>
        Task<Shipment> AddShipment(OrderImport orderImport, Order order);
    }
}
