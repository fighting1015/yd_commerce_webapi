using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.DataExporting.Excel.EpPlus;
using Vapps.ECommerce.Shippings.Importing.Dto;

namespace Vapps.ECommerce.Shippings.Importing
{
    public class ShipmentListExcelDataReader : EpPlusExcelImporterBase<ImportShipmentDto>, IShipmentListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ShipmentListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(VappsConsts.ServerSideLocalizationSourceName);
        }

        public List<ImportShipmentDto> GetShipmentsFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportShipmentDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var shipment = new ImportShipmentDto();

            try
            {
                shipment.OrderNumber = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(shipment.OrderNumber), exceptionMessage);
                shipment.LogisticsNumber = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(shipment.LogisticsNumber), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                shipment.Exception = exception.Message;
            }

            return shipment;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }


        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}无效", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
