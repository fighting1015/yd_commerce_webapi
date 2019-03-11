using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders.Exporting
{
    public interface IOrderExcelImporter
    {
        void ExportToFile(byte[] content);
    }
}
