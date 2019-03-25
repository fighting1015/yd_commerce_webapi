using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Accounts.Job;
using Vapps.Media;

namespace Vapps.Authorization.Users
{
    public class AccountSynchronizer : IEventHandler<EntityCreatedEventData<User>>,
        IEventHandler<EntityDeletedEventData<User>>,
        IEventHandler<EntityUpdatedEventData<User>>,
        IEventHandler<ExternalLoginEvent>,
        ITransientDependency
    {
        private readonly IUserAccountManager _userAccountManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountSynchronizer(
            IRepository<UserAccount, long> userAccountRepository,
            IUserAccountManager userAccountManager,
            IUnitOfWorkManager unitOfWorkManager,
            IPictureManager pictureManager,
            IBackgroundJobManager backgroundJobManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userAccountManager = userAccountManager;
            _backgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// Handles creation event of user
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(EntityCreatedEventData<User> eventData)
        {
            //using (_unitOfWorkManager.Current.SetTenantId(null))
            //{
            //    var userAccount = _userAccountManager.GetByUserId(eventData.Entity.TenantId, eventData.Entity.Id);
            //    if (userAccount == null)
            //    {
            //        _userAccountManager.CreateAsync(new Account
            //        {
            //            TenantId = eventData.Entity.TenantId,
            //            UserName = eventData.Entity.UserName,
            //            UserId = eventData.Entity.Id,
            //            EmailAddress = eventData.Entity.EmailAddress,
            //            LastLoginTime = eventData.Entity.LastLoginTime
            //        });
            //        //_unitOfWorkManager.Current.SaveChangesAsync();
            //    }
            //}
        }

        /// <summary>
        /// Handles deletion event of user
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<User> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var userAccount = _userAccountManager.GetByUserId(eventData.Entity.TenantId, eventData.Entity.Id);
                if (userAccount != null)
                {
                    _userAccountManager.DeleteAsync(userAccount);
                }
            }
        }

        /// <summary>
        /// Handles update event of user
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityUpdatedEventData<User> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var userAccount = _userAccountManager.GetByUserId(eventData.Entity.TenantId, eventData.Entity.Id);
                if (userAccount != null)
                {
                    userAccount.TenantId = eventData.Entity.TenantId;
                    userAccount.UserName = eventData.Entity.UserName;
                    userAccount.EmailAddress = eventData.Entity.EmailAddress;
                    _userAccountManager.UpdateAsync(userAccount);
                }
            }
        }

        /// <summary>
        /// Handles creation event of user
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(ExternalLoginEvent eventData)
        {
            _backgroundJobManager.Enqueue<CreateOrUpdateAccountJob, CreateOrUpdateAccountJobArgs>(new CreateOrUpdateAccountJobArgs()
            {
                UserId = eventData.User.Id,
                ExternalLoginInfo = eventData.ExternalLoginInfo,
            });
        }
    }
}
