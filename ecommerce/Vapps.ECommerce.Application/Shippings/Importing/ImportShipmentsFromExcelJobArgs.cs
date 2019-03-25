using System;
using Abp;

namespace Vapps.ECommerce.Shippings.Importing
{
    public class ImportShipmentsFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public int TenantLogisticsId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}