using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders.Dto
{
    public class GetSelectedToExcelInput
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public List<long> OrderIds { get; set; }
    }
}
