using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Vapps.Authorization.Users;

namespace Vapps.Authorization.Accounts.Job
{
    /// <summary>
    /// 更新用户信息
    /// </summary>
    public class CreateOrUpdateAccountJob : BackgroundJob<CreateOrUpdateAccountJobArgs>, ITransientDependency
    {
        private readonly UserManager _userManager;
        private readonly IUserAccountManager _userAccountManager;

        public CreateOrUpdateAccountJob(UserManager userManager,
            IUserAccountManager userAccountManager)
        {
            this._userManager = userManager;
            this._userAccountManager = userAccountManager;
        }

        [UnitOfWork]
        public override void Execute(CreateOrUpdateAccountJobArgs args)
        {
            AsyncHelper.RunSync(async () =>
            {
                var user = await _userManager.UserStore.Find4PlatformByIdAsync(args.UserId);
                await _userAccountManager.CreateOrUpdateAccountAsync(user, args.ExternalLoginInfo);
            });
        }
    }
}
