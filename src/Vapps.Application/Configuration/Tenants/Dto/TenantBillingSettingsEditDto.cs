using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Configuration.Tenants.Dto
{
    public class TenantBillingSettingsEditDto
    {
        /// <summary>
        /// 注册名称
        /// </summary>
        public string LegalName { get; set; }
        
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        public string TaxVatNo { get; set; }
    }
}
