using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using System;
using Vapps.Notifications;

namespace Vapps.WeChat.TemplateMessage.Job
{
    /// <summary>
    /// 发送模板消息
    /// </summary>
    public class SendNotificationTemplateMessageJob : BackgroundJob<SendNotificationTemplateMessageJobArgs>, ITransientDependency
    {
        private readonly ITemplateMessageSender _templateMessageSender;

        public SendNotificationTemplateMessageJob(
            ITemplateMessageSender templateMessageSender)
        {
            this._templateMessageSender = templateMessageSender;
            this.LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        [UnitOfWork]
        public override void Execute(SendNotificationTemplateMessageJobArgs args)
        {
            //AsyncHelper.RunSync(async () =>
            //{

            //    switch (args.EventType)
            //    {
            //        default:
            //            break;
            //    }
            //});
        }
    }
}
