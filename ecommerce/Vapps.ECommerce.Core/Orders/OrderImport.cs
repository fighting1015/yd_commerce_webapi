using System;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders
{

    public class OrderImport
    {
        public int StoreId { get; set; }

        public OrderSource OrderSource { get; set; }

        public string OrderNumber { get; set; }

        public string ProductSku { get; set; }

        public string PackageName { get; set; }

        public int PackageNum { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal ShipTotal { get; set; }

        public decimal Reward { get; set; }

        public string ReceiverName { get; set; }

        public string Telephone { get; set; }

        public string FullAddress { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        public string CustomerComment { get; set; }

        public string AdvertAccountId { get; set; }

        public string AdminComment { get; set; }

        public int LogisticsId { get; set; }

        public string LogisticsName { get; set; }

        public string TrackingNumber { get; set; }

        public DateTime PlaceOnUtc { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime DeliveriedOnUtc { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string OrderIp { get; set; }

        public string TrackingOrderNumber { get; set; }
    }

    public class OrderItemImport
    {
        public bool IsCombin { get; set; }

        public Product Product { get; set; }

        public ProductAttributeCombination Combin { get; set; }
        public int Quantity { get; set; }
    }

    public static class OrderImportProperties
    {

        public static string OrderNumber => "订单编号";

        public static string ProductSku => "商品编码";

        public static string PackageName => "商品名称（套餐，规格）";

        public static string OrderTotal => "价格";

        public static string ReceiverName => "收件人";

        public static string Telephone => "收件人手机号";

        public static string FullAddress => "收货地址";

        public static string Province => "省";

        public static string City => "市";

        public static string District => "区";

        public static string Address => "详细地址";

        public static string Customer => "用户留言";
    }

    public static class OrderImportCRVPropertiesIndex
    {

        public static int OrderNumber => 0;

        public static int ProductSku => 1;

        public static int PackageName => 2;

        public static int PackageNum => 3;

        public static int OrderTotal => 4;

        public static int ShipTotal => 5;

        public static int ReceiverName => 6;

        public static int Telephone => 7;

        public static int FullAddress => 8;

        public static int Province => 9;

        public static int City => 10;

        public static int District => 11;

        public static int Address => 12;

        public static int CustomerComment => 13;

        public static int AdminComment => 14;

        public static int ShipName => 15;

        public static int TrackingNumber => 16;

        public static int PlaceOrderDatetime => 17;

        public static int DeliveryDatetime => 18;

        public static int OrderStatus => 19;

        public static int TrackingOrderNumber => 21;
    }

    public static class OrderImportXlsxPropertiesIndex
    {
        public static int OrderNumber => 1;

        public static int PackageName => 2;

        public static int ProductSku => 3;

        public static int PackageNum => 4;

        public static int OrderTotal => 6;

        public static int ReceiverName => 7;

        public static int Telephone => 8;


        public static int Province => 9;

        public static int City => 10;

        public static int District => 11;

        public static int Address => 12;

        public static int PlaceOrderDatetime => 14;

        public static int OrderStatus => 15;

        public static int ShipName => 16;

        public static int TrackingNumber => 17;

        public static int CustomerComment => 18;

        public static int AdminComment => 19;
    }


    public static class OrderStatusImportValue
    {
        public static string NotYetShipped => "备货中";

        public static string Shipped => "已发货";

        public static string Canceled => "已取消";

        public static string Complated => "已完成";

        public static string Refunse => "退货中";

    }
}
