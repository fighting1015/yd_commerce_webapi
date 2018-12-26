using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using Vapps.Authorization.Users;

namespace Vapps.WeChat.Core.Users
{
    public class WeChatUserSynchronizer : IEventHandler<ExternalLoginEvent>, IEventHandler<EntityDeletedEventData<ExternalUserLogin>>, ITransientDependency
    {
        private readonly IRepository<WeChatUser, long> _weChatUserRepository;
        private readonly WeChatUserManager _weChatUserManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeChatUserSynchronizer(
            IRepository<WeChatUser, long> weChatUserRepository,
            WeChatUserManager weChatUserManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _weChatUserRepository = weChatUserRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _weChatUserManager = weChatUserManager;
        }

        /// <summary>
        /// Handles creation event of user
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(ExternalLoginEvent eventData)
        {
            if (eventData.ExternalLoginInfo.Provider != "WeChat")
                return;

            AsyncHelper.RunSync(async () =>
            {
                await _weChatUserManager.CreateOrUpdateAsync(eventData);
            });
        }

        /// <summary>
        /// Handles delete event of external user login
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<ExternalUserLogin> eventData)
        {
            AsyncHelper.RunSync(async () =>
            {
                var weChatUser = await _weChatUserManager.FindByUserIdAndMpIdAsync(eventData.Entity.UserId, 0);

                if (weChatUser != null)
                {
                    await _weChatUserRepository.DeleteAsync(weChatUser);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
            });
        }
    }
}
