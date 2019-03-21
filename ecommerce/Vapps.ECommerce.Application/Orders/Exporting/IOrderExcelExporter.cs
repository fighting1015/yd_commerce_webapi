using System.Collections.Generic;
using Vapps.Dto;
using Vapps.ECommerce.Orders.Dto;

namespace Vapps.ECommerce.Orders.Exporting
{
    public interface IOrderExcelExporter
    {
        FileDto ExportToFile(List<Order> orders);
    }
}
