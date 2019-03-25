using Abp.Dependency;
using System.Collections.Generic;
using Vapps.ECommerce.Shippings.Importing.Dto;

namespace Vapps.ECommerce.Shippings.Importing
{
    public interface IShipmentListExcelDataReader : ITransientDependency
    {
        List<ImportShipmentDto> GetShipmentsFromExcel(byte[] fileBytes);
    }
}
