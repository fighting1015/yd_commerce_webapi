using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.Timing.Timezone;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.DataExporting.Excel.EpPlus;
using Vapps.Dto;
using Vapps.ECommerce.Orders.Dto;
using Vapps.ECommerce.Products;
using Vapps.Helpers;
using Vapps.Storage;

namespace Vapps.ECommerce.Orders.Exporting
{
    public class OrderExcelExporter : EpPlusExcelExporterBase, IOrderExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IProductManager _productManager;
        private readonly IProductAttributeFormatter _productAttributeFormatter;


        public OrderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager,
            IDateTimeHelper dateTimeHelper,
            IProductManager productManager,
            IProductAttributeFormatter productAttributeFormatter)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _dateTimeHelper = dateTimeHelper;
            _productAttributeFormatter = productAttributeFormatter;
            _productManager = productManager;
        }

        [UnitOfWork]
        public FileDto ExportToFile(List<Order> orders)
        {
            var dateTimeString = Clock.Now.UtcTimeConverLocalTime(_dateTimeHelper).ToString("yyyyMMddHHmmss");

            return CreateExcelPackage(
                $"Orders-{dateTimeString}.xls",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Orders"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        3,
                        L("商家订单号"),
                        L("平台订单号"),
                        L("收件人姓名"),
                        L("收件人手机"),
                        L("收件人座机"),
                        L("收件人地址"),
                        L("收件人公司"),
                        L("物品内容"),
                        L("托寄物数量"),
                        L("包裹数量"),
                        L("参考重量（KG）"),
                        L("订单金额（元）"),
                        L("业务类型"),
                        L("生鲜温层"),
                        L("付费方式"),
                        L("是否保价"),
                        L("保价金额（元）"),
                        L("签单返还"),
                        L("是否代收货款"),
                        L("代收货款金额（元）"),
                        L("京尊达"),
                        L("防撕码收集"),
                        L("短信验证"),
                        L("备注")
                    );

                    AddObjects(
                        sheet, 4, orders,
                        _ => _.OrderNumber,
                        _ => string.Empty,
                        _ => _.ShippingName,
                        _ => _.ShippingPhoneNumber,
                        _ => string.Empty,
                        _ => _.GetFullShippingAddress(),
                        _ => string.Empty,
                        _ => GetProductName(_),
                        _ => 1,
                        _ => 1,
                        _ => 1,
                        _ => Math.Round(_.TotalAmount, 2),
                        _ => "特惠送",
                        _ => string.Empty,
                        _ => "商家月结",
                        _ => "否",
                        _ => string.Empty,
                        _ => "否",
                        _ => "是",
                        _ => Math.Round(_.TotalAmount, 2),
                        _ => string.Empty,
                        _ => string.Empty,
                        _ => string.Empty,
                        _ => GetOrderRemark(_).Result
                        );

                    //Formatting cells
                    //var timeColumn = sheet.Column(1);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }


        private string GetProductName(Order order)
        {
            List<string> names = new List<string>();

            order.Items.Each(o => names.Add(o.ProductName));
            if (names.Any())
                return string.Join(",", names);

            return string.Empty;
        }

        private async Task<string> GetOrderRemark(Order order)
        {
            string remark = "可开箱验货";

            if (!order.Items.Any())
                return remark;

            string productDetail = string.Empty;
            foreach (var item in order.Items.ToList())
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);
                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(item.AttributesJson);
                string productName = item.ProductName;

                if (!item.AttributesJson.IsNullOrEmpty())
                    productName = productName + " " + await _productAttributeFormatter.FormatAttributesAsync(product, jsonAttributeList, " ");

                if (productDetail == string.Empty)
                    productDetail = productDetail + $"{productName} * {item.Quantity}";
                else
                    productDetail = productDetail + $"；{productName} * {item.Quantity}";
            }

            remark = $"{remark.Trim()} ({productDetail.Trim()};{order.CustomerComment})";

            return remark;
        }
    }
}
