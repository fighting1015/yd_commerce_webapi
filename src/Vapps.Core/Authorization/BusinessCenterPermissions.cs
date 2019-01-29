﻿using Vapps.Security;

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

        [Permission(Self)]
        public class Organization
        {
            public const string Self = "BusiCenter.Organization";

            public const string BaseInfo = "BusiCenter.Organization.BaseInfo";

            [Permission(Self)]
            public class Outlets
            {
                public const string Self = "BusiCenter.Organization.Outlets";

                public const string Create = "BusiCenter.Organization.Outlet.Create";
                public const string Edit = "BusiCenter.Organization.Outlet.Edit";
                public const string Delete = "BusiCenter.Organization.Outlet.Delete";
            }

            [Permission(Self)]
            public class Contactors
            {
                public const string Self = "BusiCenter.Organization.Contactors";

                public const string Create = "BusiCenter.Organization.Contactor.Create";
                public const string Edit = "BusiCenter.Organization.Contactor.Edit";
                public const string Delete = "BusiCenter.Organization.Contactor.Delete";
            }
        }

        [Permission(Self)]
        public class ECommerce
        {
            public const string Self = "BusiCenter.ECommerce";

            [Permission(Self)]
            public class Category
            {
                public const string Self = "BusiCenter.ECommerce.Category";

                public const string Create = "BusiCenter.ECommerce.Category.Create";
                public const string Edit = "BusiCenter.ECommerce.Category.Edit";
                public const string Delete = "BusiCenter.ECommerce.Category.Delete";
            }

            [Permission(Self)]
            public class Product
            {
                public const string Self = "BusiCenter.ECommerce.Product";

                public const string Create = "BusiCenter.ECommerce.Product.Create";
                public const string Edit = "BusiCenter.ECommerce.Product.Edit";
                public const string Delete = "BusiCenter.ECommerce.Product.Delete";
            }
        }


        [Permission(Self)]
        public class ContentManage
        {
            public const string Self = "BusiCenter.ContentManage";

            [Permission(Self)]
            public class PictureGallery
            {
                public const string Self = "BusiCenter.ContentManage.PictureGallery";

                public const string GroupCreate = "BusiCenter.ContentManage.PictureGallery.Group.Create";
                public const string GroupEdit = "BusiCenter.ContentManage.PictureGallery.Group.Edit";
                public const string GroupDelete = "BusiCenter.ContentManage.PictureGallery.Group.Delete";
            }

            [Permission(Self)]
            public class Pictures
            {
                public const string Self = "BusiCenter.ContentManage.Pictures";

                public const string Create = "BusiCenter.ContentManage.Picture.Create";
                public const string Edit = "BusiCenter.ContentManage.Picture.Edit";
                public const string Delete = "BusiCenter.ContentManage.Picture.Delete";
            }
        }
    }
}
