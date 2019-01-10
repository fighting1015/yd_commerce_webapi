using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
using Vapps.Authorization.Users;
using Vapps.MultiTenancy;
using Vapps.Authorization.Accounts;
using Vapps.Identity;
using Vapps.Authorization.Accounts.Cache;
using Abp.Auditing;

namespace Vapps
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class VappsAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public UserAccountManager UserAccountManager { get; set; }

        public IAccountCache AccountCache { get; set; }

        protected VappsAppServiceBase()
        {
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual async Task<User> GetCurrentUserIfLoginAsync()
        {
            if (!AbpSession.UserId.HasValue)
                return null;

            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual async Task<AccountCacheItem> GetCurrentUserAccountCacheAsync()
        {
            var userAccount = await AccountCache.GetAsync(AbpSession.GetUserId());
            if (userAccount == null)
            {
                throw new Exception("There is no current user!");
            }

            return userAccount;
        }

        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}