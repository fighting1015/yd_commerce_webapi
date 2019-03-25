using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.DataExporting.Excel.EpPlus;
using Vapps.Dto;
using Vapps.ECommerce.Shippings.Importing.Dto;
using Vapps.Helpers;
using Vapps.Storage;

namespace Vapps.ECommerce.Shippings.Importing
{
    public class InvalidShipmentExporter : EpPlusExcelExporterBase, IInvalidShipmentExporter
    {
        private readonly IDateTimeHelper _dateTimeHelper;

        public InvalidShipmentExporter(IDateTimeHelper dateTimeHelper,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            this._dateTimeHelper = dateTimeHelper;
        }

        public FileDto ExportToFile(List<ImportShipmentDto> userListDtos)
        {
            var dateTimeString = Clock.Now.UtcTimeConverLocalTime(_dateTimeHelper).ToString("yyyyMMddHHmmss");

            return CreateExcelPackage(
                $"{L("无效物流单号")}-{dateTimeString}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidUserImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserName"),
                        L("Name"),
                        L("Exception")
                        );

                    AddObjects(
                        sheet, 2, userListDtos,
                        _ => _.OrderNumber,
                        _ => _.LogisticsNumber,
                        _ => _.Exception
                        );

                    for (var i = 1; i <= 9; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
