using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using Vapps.Authorization.Users;
using Vapps.MultiTenancy;

namespace Vapps.Notifications
{
    public class AppNotifier : VappsDomainServiceBase, IAppNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }

        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    VappsConsts.ServerSideLocalizationSourceName
                    )
                );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }

        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    VappsConsts.ServerSideLocalizationSourceName
                    )
                );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                "订单导入成功通知",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }

        //This is for test purposes
        public async Task SendMessageAsync(UserIdentifier user, string notificationName, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                notificationName,
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }

        public async Task SomeShipmentsCouldntBeImported(UserIdentifier argsUser, string fileToken, string fileType, string fileName)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "ClickToSeeInvalidShipments",
                    VappsConsts.ServerSideLocalizationSourceName
                )
            );

            notificationData["fileToken"] = fileToken;
            notificationData["fileType"] = fileType;
            notificationData["fileName"] = fileName;

            await _notificationPublisher.PublishAsync(AppNotificationNames.DownloadInvalidImportShipments, notificationData, userIds: new[] { argsUser });
        }
    }
}