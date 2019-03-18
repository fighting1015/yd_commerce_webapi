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
        public class Order
        {
            public const string Self = "BusiCenter.Order";

            public const string Create = "BusiCenter.Order.Create";
            public const string Edit = "BusiCenter.Order.Edit";
            public const string Delete = "BusiCenter.Order.Delete";

            public const string Import = "BusiCenter.Order.Import";
            public const string Export = "BusiCenter.Order.Export";
        }

        [Permission(Self, MultiTenancySides.Tenant)]
        public class Shipment
        {
            public const string Self = "BusiCenter.Shipment";

            public const string Create = "BusiCenter.Shipment.Create";
            public const string Edit = "BusiCenter.Shipment.Edit";
            public const string Delete = "BusiCenter.Shipment.Delete";
        }

        [Permission(Self, MultiTenancySides.Host)]
        public class Logistics
        {
            public const string Self = "BusiCenter.Logistics";

            public const string Create = "BusiCenter.Logistics.Create";
            public const string Edit = "BusiCenter.Logistics.Edit";
            public const string Delete = "BusiCenter.Logistics.Delete";
        }

        [Permission(Self, MultiTenancySides.Tenant)]
        public class TenantLogistics
        {
            public const string Self = "BusiCenter.TenantLogistics";

            public const string Create = "BusiCenter.TenantLogistics.Create";
            public const string Edit = "BusiCenter.TenantLogistics.Edit";
            public const string Delete = "BusiCenter.TenantLogistics.Delete";
        }

        [Permission(Self, MultiTenancySides.Tenant)]
        public class Customer
        {
            public const string Self = "BusiCenter.Customer";

            public const string Create = "BusiCenter.Customer.Create";
            public const string Edit = "BusiCenter.Customer.Edit";
            public const string Delete = "BusiCenter.Customer.Delete";

            public const string Export = "BusiCenter.Customer.Export";
        }

        [Permission(Self, MultiTenancySides.Tenant)]
        public class Store
        {
            public const string Self = "BusiCenter.Store";

            public const string Create = "BusiCenter.Store.Create";
            public const string Edit = "BusiCenter.Store.Edit";
            public const string Delete = "BusiCenter.Store.Delete";
        }

        [Permission(Self)]
        public class Content
        {
            public const string Self = "BusiCenter.Content";

            [Permission(Self, MultiTenancySides.Tenant)]
            public class PictureGroup
            {
                public const string Self = "BusiCenter.Content.PictureGroup";

                public const string Create = "BusiCenter.Content.PictureGallery.Group.Create";
                public const string Edit = "BusiCenter.Content.PictureGallery.Group.Edit";
                public const string Delete = "BusiCenter.Content.PictureGallery.Group.Delete";
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
