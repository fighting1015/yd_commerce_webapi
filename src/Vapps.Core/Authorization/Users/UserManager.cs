using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.ExternalAuthentications;

namespace Vapps.Authorization.Users
{
    /// <summary>
    /// User manager.
    /// Used to implement domain logic for users.
    /// Extends <see cref="AbpUserManager{TRole,TUser}"/>.
    /// </summary>
    public class UserManager : AbpUserManager<Role, User>
    {
        public UserStore UserStore;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ILocalizationManager _localizationManager;

        public UserManager(
            UserStore userStore,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager> logger,
            RoleManager roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            ISettingManager settingManager)
            : base(
                  roleManager,
                  userStore,
                  optionsAccessor,
                  passwordHasher,
                  null,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger,
                  permissionManager,
                  unitOfWorkManager,
                  cacheManager,
                  organizationUnitRepository,
                  userOrganizationUnitRepository,
                  organizationUnitSettings,
                  settingManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _localizationManager = localizationManager;
            UserStore = userStore;

            this.UserValidators.Add(new VappsUserValidator<User>(localizationManager));
        }

        #region Utilities

        private new string L(string name)
        {
            return LocalizationManager.GetString(VappsConsts.ServerSideLocalizationSourceName, name);
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

        #endregion

        #region User

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plainPassword">明文密码</param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(User user, string plainPassword)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressOrPhoneNumberAsync(user.Id, user.UserName, user.EmailAddress, user.PhoneNumber);
            if (!result.Succeeded)
            {
                return result;
            }

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            return await base.CreateAsync(user, plainPassword);
        }

        [UnitOfWork]
        public virtual async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                return await FindByIdAsync(userIdentifier.UserId.ToString());
            }
        }

        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new Exception("There is no user: " + userIdentifier);
            }

            return user;
        }

        public virtual User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }

        public virtual async Task<User> FindByTelephoneAsync(string phoneNumber)
        {
            return await UserStore.FindByPhoneNumberAsync(phoneNumber);
        }

        /// <summary>
        /// 更新用户(全平台)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Update4PlatformAsync(User user)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UpdateAsync(user);
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public async override Task<IdentityResult> UpdateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressOrPhoneNumberAsync(user.Id, user.UserName, user.EmailAddress, user.PhoneNumber);
            if (!result.Succeeded)
            {
                return result;
            }

            //Admin user's username can not be changed!
            if (user.UserName != User.AdminUserName)
            {
                if ((await GetOldUserNameAsync(user.Id)) == User.AdminUserName)
                {
                    throw new UserFriendlyException(string.Format(L("CanNotRenameAdminUser"), AbpUserBase.AdminUserName));
                }
            }

            return await base.UpdateAsync(user);
        }

        /// <summary>
        /// 检查用户名/邮箱 是否存在
        /// </summary>
        /// <param name="expectedUserId"></param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateUserName"), userName));
            }

            if (!emailAddress.IsNullOrEmpty())
            {
                user = (await FindByEmailAsync(emailAddress));
                if (user != null && user.Id != expectedUserId)
                {
                    throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), emailAddress));
                }
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// 检查用户名/邮箱/手机是否存在
        /// </summary>
        /// <param name="expectedUserId"></param>
        /// <param name="userName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressOrPhoneNumberAsync(long? expectedUserId,
            string userName, string emailAddress, string phoneNumber)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateName"), userName));
            }

            if (!emailAddress.IsNullOrEmpty())
            {
                user = (await FindByEmailAsync(emailAddress));
                if (user != null && user.Id != expectedUserId)
                {
                    throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), emailAddress));
                }
            }

            if (!phoneNumber.IsNullOrEmpty())
            {
                if (!phoneNumber.IsMobilePhone())
                {
                    throw new UserFriendlyException(string.Format(L("Identity.InvaildPhoneNumber"), phoneNumber));
                }

                user = (await UserStore.FindByPhoneNumberAsync(phoneNumber));
                if (user != null && user.Id != expectedUserId)
                {
                    throw new UserFriendlyException(string.Format(L("Identity.DuplicatePhoneNumber"), phoneNumber));
                }
            }
            return IdentityResult.Success;
        }

        public override Task<IdentityResult> SetRoles(User user, string[] roleNames)
        {
            if (user.Name == "admin" && !roleNames.Contains(StaticRoleNames.Host.Admin))
            {
                throw new UserFriendlyException(L("AdminRoleCannotRemoveFromAdminUser"));
            }

            return base.SetRoles(user, roleNames);
        }

        public override async Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            CheckPermissionsToUpdate(user, permissions);

            await base.SetGrantedPermissionsAsync(user, permissions);
        }

        private void CheckPermissionsToUpdate(User user, IEnumerable<Permission> permissions)
        {
            if (user.Name == AbpUserBase.AdminUserName &&
                (!permissions.Any(p => p.Name == AdminPermissions.UserManage.Roles.Edit) ||
                !permissions.Any(p => p.Name == AdminPermissions.UserManage.Roles.ChangePermissions)))
            {
                throw new UserFriendlyException(L("YouCannotRemoveUserRolePermissionsFromAdminUser"));
            }
        }

        #endregion

        #region IUserLoginStore

        /// <summary>
        /// 更新外部认证信息
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public virtual async Task UpdateUserLoginAsync(ExternalLoginUserInfo loginInfo)
        {
            var userLogin = await UserStore.FindByExternalLoginInfoAsync(loginInfo);

            userLogin.AccessToken = loginInfo.AccessToken;
            userLogin.AccessTokenOutDataTime = loginInfo.AccessTokenOutDataTime;
            userLogin.RefreshToken = loginInfo.RefreshToken;

            await UserStore.UpdateUserLoginAsync(userLogin);
        }

        #endregion
    }
}