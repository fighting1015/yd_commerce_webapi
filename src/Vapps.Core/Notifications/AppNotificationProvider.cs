using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using Vapps.Authorization;

namespace Vapps.Notifications
{
    public class AppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewUserRegistered,
                    displayName: L("NewUserRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AdminPermissions.UserManage.Self)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTenantRegistered,
                    displayName: L("NewTenantRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AdminPermissions.UserManage.Tenants.Self)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, VappsConsts.ServerSideLocalizationSourceName);
        }
    }
}
