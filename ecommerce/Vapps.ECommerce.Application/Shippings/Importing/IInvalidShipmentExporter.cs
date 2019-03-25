using System.Collections.Generic;
using Vapps.Dto;
using Vapps.ECommerce.Shippings.Importing.Dto;

namespace Vapps.ECommerce.Shippings.Importing
{
    public interface IInvalidShipmentExporter
    {
        FileDto ExportToFile(List<ImportShipmentDto> shipmentListDtos);
    }
}
