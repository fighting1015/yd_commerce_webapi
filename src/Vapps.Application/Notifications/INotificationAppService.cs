using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Vapps.Notifications.Dto;

namespace Vapps.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);

        /// <summary>
        /// 设置所有通知为已读
        /// </summary>
        /// <returns></returns>
        Task SetAllNotificationsAsRead();

        /// <summary>
        /// 设置通知为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetNotificationAsRead(EntityDto<Guid> input);

        /// <summary>
        /// 获取通知设置
        /// </summary>
        /// <returns></returns>
        Task<GetNotificationSettingsOutput> GetNotificationSettings();

        /// <summary>
        /// 更新通知设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteNotification(EntityDto<Guid> input);
    }
}