namespace Vapps.ECommerce.Orders.Toutiao
{
    public static class ApiName
    {
        /// <summary>
        /// 添加规格
        /// </summary>
        public static string SpecAdd => "spec.add";

        /// <summary>
        /// 查看规格详细
        /// </summary>
        public static string SpecDetail => "spec.specDetail";

        /// <summary>
        /// 查看规格列表
        /// </summary>
        public static string SpecList => "spec.list";

        /// <summary>
        /// 删除规格
        /// </summary>
        public static string SpecDel => "spec.del";

        /// <summary>
        /// 订单列表
        /// </summary>
        public static string OrderList => "order.list";

        /// <summary>
        /// 订单详情
        /// </summary>
        public static string OrderDetail => "order.detail";

        /// <summary>
        /// 获取快递公司列表
        /// </summary>
        public static string LogisticsCompanyList => "order.logisticsCompanyList";

        /// <summary>
        /// 订单确认
        /// </summary>
        public static string StockUp => "order.stockUp";

        /// <summary>
        /// 订单出库
        /// </summary>
        public static string LogisticsAdd => "order.logisticsAdd";

        /// <summary>
        /// 订单物流修改 (或者重新发货)
        /// </summary>
        public static string logisticsEdit => "order.logisticsEdit";
    }

    public static class ApiAddress
    {
        /// <summary>
        /// 添加规格
        /// </summary>
        public static string SpecAdd => "https://openapi.jinritemai.com/spec/specDetail";

        /// <summary>
        /// 查看规格详细
        /// </summary>
        public static string SpecDetail => "https://openapi.jinritemai.com/spec/specDetail";

        /// <summary>
        /// 查看规格列表
        /// </summary>
        public static string SpecList => "https://openapi.jinritemai.com/spec/del";

        /// <summary>
        /// 删除规格
        /// </summary>
        public static string SpecDel => "https://openapi.jinritemai.com/spec/del";

        /// <summary>
        /// 订单列表
        /// </summary>
        public static string OrderList => "https://openapi.jinritemai.com/order/list";

        /// <summary>
        /// 订单详情
        /// </summary>
        public static string OrderDetail => "https://openapi.jinritemai.com/order/list";

        /// <summary>
        /// 获取快递公司列表
        /// </summary>
        public static string LogisticsCompanyList => "https://openapi.jinritemai.com/order/list";

        /// <summary>
        /// 订单确认
        /// </summary>
        public static string StockUp => "https://openapi.jinritemai.com/order/stockUp";

        /// <summary>
        /// 订单出库
        /// </summary>
        public static string LogisticsAdd => "https://openapi.jinritemai.com/order/logisticsAdd";

        /// <summary>
        /// 订单物流修改 (或者重新发货)
        /// </summary>
        public static string logisticsEdit => "https://openapi.jinritemai.com/order/logisticsEdit";
    }
}
