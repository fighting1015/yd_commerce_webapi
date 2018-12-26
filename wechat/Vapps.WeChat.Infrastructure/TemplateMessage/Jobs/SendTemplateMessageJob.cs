using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;

namespace Vapps.WeChat.TemplateMessage.Job
{
    /// <summary>
    /// 发送模板消息
    /// </summary>
    public class SendTemplateMessageJob : BackgroundJob<SendTemplateMessageJobArgs>, ITransientDependency
    {
        private readonly ITemplateMessageSender _templateMessageSender;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SendTemplateMessageJob(
            ITemplateMessageSender templateMessageSender,
            IUnitOfWorkManager unitOfWorkManager)
        {
            this._templateMessageSender = templateMessageSender;
            this._unitOfWorkManager = unitOfWorkManager;
            this.LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        [UnitOfWork]
        public override void Execute(SendTemplateMessageJobArgs args)
        {
            //AsyncHelper.RunSync(async () =>
            //{
                
            //});
        }
    }
}
