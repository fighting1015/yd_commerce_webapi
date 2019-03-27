using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings.Jobs
{
    public class ProcessShipJobArg
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 发货记录Id
        /// </summary>
        public long ShipmentId { get; set; }
    }
}
