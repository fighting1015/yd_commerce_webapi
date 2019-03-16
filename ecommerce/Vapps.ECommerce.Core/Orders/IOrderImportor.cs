using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
