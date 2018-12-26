using Abp.MultiTenancy;
using Vapps.Security;

namespace Vapps.Authorization
{
    /// <summary>
    /// 管理系统权限配置类
    /// 权限常量命名命名规范：{父级权限名称}.{当前权限字段.ToCamelCase()}
    /// 内部类中Self字段用于标识本身的权限名称
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public class AdminPermissions
    {
        public const string Self = "Admin";

        [Permission(HostDashboard, MultiTenancySides.Host)]
        public const string HostDashboard = "Admin.Host.Dashboard";

        [Permission(TenantDashboard, MultiTenancySides.Tenant)]
        public const string TenantDashboard = "Admin.Tenant.Dashboard";

        [Permission(Self)]
        public class UserManage
        {

            public const string Self = "Admin.UserManage";

            [Permission(Self, MultiTenancySides.Host)]
            public class Tenants
            {
                public const string Self = "Admin.UserManage.Tenants";

                public const string Create = "Admin.UserManage.Tenants.Create";
                public const string Edit = "Admin.UserManage.Tenants.Edit";
                public const string Delete = "Admin.UserManage.Tenants.Delete";

                public const string ChangeFeatures = "Admin.UserManage.Tenants.ChangeFeatures";
                public const string Impersonation = "Admin.UserManage.Tenants.Impersonation";
            }

            [Permission(Self)]
            public class OrganizationUnits
            {
                public const string Self = "Admin.UserManage.OrganizationUnits";

                public const string ManageOrganizationTree = "Admin.UserManage.OrganizationUnits.ManageOrganizationTree";
                public const string ManageMembers = "Admin.UserManage.OrganizationUnits.ManageMembers";
            }

            [Permission(Self)]
            public class Roles
            {
                public const string Self = "Admin.UserManage.Roles";

                public const string Create = "Admin.UserManage.Roles.Create";
                public const string Edit = "Admin.UserManage.Roles.Edit";
                public const string Delete = "Admin.UserManage.Roles.Delete";

                public const string ChangePermissions = "Admin.UserManage.Roles.ChangePermissions";
            }

            [Permission(Self)]
            public class Users
            {
                public const string Self = "Admin.UserManage.Users";

                public const string Create = "Admin.UserManage.Users.Create";
                public const string Edit = "Admin.UserManage.Users.Edit";
                public const string Delete = "Admin.UserManage.Users.Delete";

                public const string SetRole = "Admin.UserManage.Users.SetRole";
                public const string ChangePermissions = "Admin.UserManage.Users.ChangePermissions";
                public const string Impersonation = "Admin.UserManage.Users.Impersonation";
            }
        }

        [Permission(Self)]
        public class Configuration
        {
            public const string Self = "Admin.Configuration";

            [Permission(Self)]
            public class Languages
            {
                public const string Self = "Admin.Configuration.Languages";

                public const string Create = "Admin.Configuration.Languages.Create";
                public const string Edit = "Admin.Configuration.Languages.Edit";
                public const string Delete = "Admin.Configuration.Languages.Delete";

                public const string ChangeTexts = "Admin.Configuration.Languages.ChangeTexts";
            }

            [Permission(TenantSettings, MultiTenancySides.Tenant)]
            public const string TenantSettings = "Admin.Configuration.Tenant.Settings";

            [Permission(TenantSubscriptionManagement, MultiTenancySides.Tenant)]
            public const string TenantSubscriptionManagement = "Admin.Configuration.Tenant.SubscriptionManagement";

            [Permission(HostSettings, MultiTenancySides.Host)]
            public const string HostSettings = "Admin.Configuration.Host.Settings";

           
        }

        [Permission(Self)]
        public class ContentManage
        {
            public const string Self = "Admin.ContentManage";

            [Permission(Self, MultiTenancySides.Host)]
            public class SmsTemplates
            {
                public const string Self = "Admin.ContentManage.SmsTemplates";

                public const string Create = "Admin.ContentManage.SmsTemplates.Create";
                public const string Edit = "Admin.ContentManage.SmsTemplates.Edit";
                public const string Delete = "Admin.ContentManage.SmsTemplates.Delete";
            }
        }

        [Permission(Self)]
        public class System
        {
            public System() { }
            public const string Self = "Admin.System";

            [Permission(Self)]
            public class Logs
            {
                public const string Self = "Admin.System.Logs";

                public const string LoginLogs = "Admin.System.Logs.LoginLogs";

                [Permission(Self)]
                public class AuditLogs
                {
                    public const string Self = "Admin.System.Logs.AuditLogs";
                    public const string Delete = "Admin.System.Logs.AuditLogs.Delete";
                }
            }

            [Permission(HostMaintenance, MultiTenancySides.Host, true)]
            public const string HostMaintenance = "Admin.System.Host.Maintenance";

            [Permission(HangfireDashboard, MultiTenancySides.Host, true)]
            public const string HangfireDashboard = "Admin.System.HangfireDashboard";

            [Permission(UiCustomization)]
            public const string UiCustomization = "Admin.System.UiCustomization";


            [Permission(Self, MultiTenancySides.Host)]
            public class Editions
            {
                public const string Self = "Admin.System.Editions";

                public const string Create = "Admin.System.Editions.Create";
                public const string Edit = "Admin.System.Editions.Edit";
                public const string Delete = "Admin.System.Editions.Delete";
            }
        }
    }
}