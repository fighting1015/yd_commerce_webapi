using Abp.MultiTenancy;
using Vapps.Security;

namespace Vapps.Authorization
{
    /// <summary>
    /// 机构中心权限配置类
    /// 权限常量命名命名规范：{父级权限名称}.{当前权限字段.ToCamelCase()}
    /// 内部类中Self字段用于标识本身的权限名称
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public class BusinessCenterPermissions
    {
        public const string Self = "BusiCenter";

        public const string Dashboard = "BusiCenter.Dashboard";


        [Permission(Self, MultiTenancySides.Tenant)]
        public class Catelog
        {
            public const string Self = "BusiCenter.Catelog";

            public class Category
            {
                public const string Self = "BusiCenter.Catelog.Category";

                public const string Create = "BusiCenter.Catelog.Category.Create";
                public const string Edit = "BusiCenter.Catelog.Category.Edit";
                public const string Delete = "BusiCenter.Catelog.Category.Delete";
            }

            public class Product
            {
                public const string Self = "BusiCenter.Catelog.Product";

                public const string Create = "BusiCenter.Catelog.Product.Create";
                public const string Edit = "BusiCenter.Catelog.Product.Edit";
                public const string Delete = "BusiCenter.Catelog.Product.Delete";
            }

            public class ProductAttribute
            {
                public const string Self = "BusiCenter.Catelog.ProductAttribute";

                public const string Create = "BusiCenter.Catelog.ProductAttribute.Create";
                public const string Edit = "BusiCenter.Catelog.ProductAttribute.Edit";
                public const string Delete = "BusiCenter.Catelog.ProductAttribute.Delete";
            }
        }

        [Permission(Self, MultiTenancySides.Tenant)]
        public class SalesManage
        {
            public const string Self = "BusiCenter.SalesManage";

            [Permission(Self, MultiTenancySides.Tenant)]
            public class Order
            {
                public const string Self = "BusiCenter.SalesManage.Order";

                public const string Create = "BusiCenter.SalesManage.Order.Create";
                public const string Edit = "BusiCenter.SalesManage.Order.Edit";
                public const string Delete = "BusiCenter.SalesManage.Order.Delete";

                public const string Import = "BusiCenter.SalesManage.Order.Import";
                public const string Export = "BusiCenter.SalesManage.Order.Export";
            }

            [Permission(Self, MultiTenancySides.Tenant)]
            public class Shipment
            {
                public const string Self = "BusiCenter.SalesManage.Shipment";

                public const string Create = "BusiCenter.SalesManage.Shipment.Create";
                public const string Edit = "BusiCenter.SalesManage.Shipment.Edit";
                public const string Delete = "BusiCenter.SalesManage.Shipment.Delete";

                public const string Import = "BusiCenter.SalesManage.Shipment.Import";
            }
        }


        [Permission(Self, MultiTenancySides.Tenant)]
        public class CustomerManage
        {
            public const string Self = "BusiCenter.CustomerManage";

            [Permission(Self, MultiTenancySides.Tenant)]
            public class Customer
            {
                public const string Self = "BusiCenter.CustomerManage.Customer";

                public const string Create = "BusiCenter.CustomerManage.Customer.Create";
                public const string Edit = "BusiCenter.CustomerManage.Customer.Edit";
                public const string Delete = "BusiCenter.CustomerManage.Customer.Delete";

                public const string Export = "BusiCenter.CustomerManage.Customer.Export";
            }
        }

        [Permission(Self)]
        public class StoreConfiguration
        {
            public const string Self = "BusiCenter.StoreConfiguration";

            [Permission(Self, MultiTenancySides.Tenant)]
            public class Store
            {
                public const string Self = "BusiCenter.StoreConfiguration.Store";

                public const string Create = "BusiCenter.StoreConfiguration.Store.Create";
                public const string Edit = "BusiCenter.StoreConfiguration.Store.Edit";
                public const string Delete = "BusiCenter.StoreConfiguration.Store.Delete";
            }

            [Permission(Self, MultiTenancySides.Host)]
            public class Logistics
            {
                public const string Self = "BusiCenter.StoreConfiguration.Logistics";

                public const string Create = "BusiCenter.StoreConfiguration.Logistics.Create";
                public const string Edit = "BusiCenter.StoreConfiguration.Logistics.Edit";
                public const string Delete = "BusiCenter.StoreConfiguration.Logistics.Delete";
            }

            [Permission(Self, MultiTenancySides.Tenant)]
            public class TenantLogistics
            {
                public const string Self = "BusiCenter.StoreConfiguration.TenantLogistics";

                public const string Create = "BusiCenter.StoreConfiguration.TenantLogistics.Create";
                public const string Edit = "BusiCenter.StoreConfiguration.TenantLogistics.Edit";
                public const string Delete = "BusiCenter.StoreConfiguration.TenantLogistics.Delete";
            }
        }

        [Permission(Self)]
        public class Content
        {
            public const string Self = "BusiCenter.Content";

            [Permission(Self, MultiTenancySides.Tenant)]
            public class PictureGroup
            {
                public const string Self = "BusiCenter.Content.PictureGroup";

                public const string Create = "BusiCenter.Content.PictureGroup.Create";
                public const string Edit = "BusiCenter.Content.PictureGroup.Edit";
                public const string Delete = "BusiCenter.Content.PictureGroup.Delete";
            }

            [Permission(Self, MultiTenancySides.Tenant)]
            public class Picture
            {
                public const string Self = "BusiCenter.Content.Picture";

                public const string Create = "BusiCenter.Content.Picture.Create";
                public const string Edit = "BusiCenter.Content.Picture.Edit";
                public const string Delete = "BusiCenter.Content.Picture.Delete";
            }
        }
    }
}
