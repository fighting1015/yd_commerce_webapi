using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.ExternalAuthentications;
using System.Linq;
using Abp.Extensions;
using System;

namespace Vapps.Authorization.Users
{
    /// <summary>
    /// Used to perform database operations for <see cref="UserManager"/>.
    /// </summary>
    public class UserStore : AbpUserStore<Role, User>
    {
        private readonly IRepository<ExternalUserLogin, long> _externalUserLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserClaim, long> _userClaimRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionSettingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserStore(
            IRepository<User, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<ExternalUserLogin, long> externalUserLoginRepository,
            IRepository<Role> roleRepository,
            IAsyncQueryableExecuter asyncQueryableExecuter,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userCliamRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository)
            : base(
                unitOfWorkManager,
                userRepository,
                roleRepository,
                asyncQueryableExecuter,
                userRoleRepository,
                userLoginRepository,
                userCliamRepository,
                userPermissionSettingRepository)
        {
            this._userRoleRepository = userRoleRepository;
            this._userClaimRepository = userCliamRepository;
            this._roleRepository = roleRepository;
            this._userPermissionSettingRepository = userPermissionSettingRepository;
            this._unitOfWorkManager = unitOfWorkManager;
            this._externalUserLoginRepository = externalUserLoginRepository;
        }

        #region IUserStore

        public virtual async Task<User> Find4PlatformByIdAsync(long userId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(userId);
            }
        }

        public virtual async Task<User> Find4PlatformByNameAsync(string userName)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(user => user.UserName == userName);
            }
        }

        public virtual async Task<User> Find4PlatformByEmailAsync(string email)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(user => user.EmailAddress == email);
            }
        }
        public virtual async Task<User> FindMainUser4PlatformByTenantIdAsync(int tenantId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(user => user.TenantId == tenantId && user.IsMainUser);
            }
        }

        /// <summary>
        /// Tries to find a user with user name or email address in current tenant.
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<User> Find4PlatformByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress));
            }
        }

        /// <summary>
        /// 根据手机获取用户
        /// </summary>
        /// <param name="loginCertificateAsync">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<User> FindByPhoneNumberAsync(string phonenumber)
        {
            return await UserRepository.FirstOrDefaultAsync(user => user.PhoneNumber == phonenumber);
        }

        /// <summary>
        /// 根据手机获取平台用户
        /// </summary>
        /// <param name="loginCertificateAsync">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<User> Find4PlatformByPhoneNumberAsync(string phonenumber)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await UserRepository.FirstOrDefaultAsync(user => user.PhoneNumber == phonenumber);
            }
        }

        /// <summary>
        /// 在当前租户下使用登陆凭证(用户名/邮箱/手机)获取用户
        /// </summary>
        /// <param name="loginCertificateAsync">User name or email address</param>
        /// <returns>User or null</returns>
        public virtual async Task<User> FindByLoginCertificateAsync(string loginCertificateAsync)
        {
            return await UserRepository.FirstOrDefaultAsync(user => (user.UserName == loginCertificateAsync ||
            user.EmailAddress == loginCertificateAsync || user.PhoneNumber == loginCertificateAsync));
        }

        /// <summary>
        /// 在指定租户下使用登陆凭证(用户名/邮箱/手机)获取用户
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public virtual async Task<User> FindByLoginCertificateAsync(int? tenantId, string loginCertificateAsync)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await FindByLoginCertificateAsync(loginCertificateAsync);
            }
        }

        /// <summary>
        /// 使用登陆凭证(用户名/邮箱/手机)获取用户(所有租户)
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="userNameOrEmailAddress">User name or email address</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public virtual async Task<User> FindByLoginCertificate4PlatformAsync(string loginCertificateAsync)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return await FindByLoginCertificateAsync(loginCertificateAsync);
            }
        }

        #endregion

        #region IUserLoginStore

        /// <summary>
        /// 当前账号是否绑定外部登陆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <returns></returns>
        public virtual async Task<bool> IfUserAssociateExternal(User user, string loginProvider)
        {
            if (user == null)
                throw new Exception("User can not be null");

            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);

            return user.Logins.Any(l => l.LoginProvider == loginProvider);
        }

        /// <summary>
        /// 当前账号是否绑定外部登陆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginProvider"></param>
        /// <returns></returns>
        public virtual async Task<ExternalUserLogin> GetAssociateExternal(User user, string loginProvider)
        {
            if (user == null)
                throw new Exception("User can not be null");

            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);

            return (ExternalUserLogin)user.Logins.FirstOrDefault(l => l.LoginProvider == loginProvider);
        }

        /// <summary>
        /// 获取认证信息
        /// </summary>
        /// <param name="externalLoginUserInfo"></param>
        /// <returns></returns>
        public virtual async Task<ExternalUserLogin> FindByExternalLoginInfoAsync(ExternalLoginUserInfo externalLoginUserInfo)
        {
            ExternalUserLogin userLogin;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                if (!externalLoginUserInfo.UnionProviderKey.IsNullOrWhiteSpace())
                {
                    userLogin = await _externalUserLoginRepository.FirstOrDefaultAsync(ul => ul.LoginProvider == externalLoginUserInfo.Provider &&
                    ul.UnionProviderKey == externalLoginUserInfo.UnionProviderKey);
                }
                else
                {
                    userLogin = await _externalUserLoginRepository.FirstOrDefaultAsync(ul => ul.LoginProvider == externalLoginUserInfo.Provider &&
                    ul.ProviderKey == externalLoginUserInfo.ProviderKey);
                }

                return userLogin;
            }
        }

        /// <summary>
        /// 更新外部认证信息
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual async Task UpdateUserLoginAsync(ExternalUserLogin login)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                await _externalUserLoginRepository.UpdateAsync(login);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public virtual async Task<User> Find4PlatformAsync(ExternalUserLoginInfo login)
        {
            ExternalUserLogin userLogin;
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                if (!login.UnionProviderKey.IsNullOrWhiteSpace())
                {
                    userLogin = await _externalUserLoginRepository.FirstOrDefaultAsync(ul => ul.LoginProvider == login.LoginProvider &&
                    ul.UnionProviderKey == login.UnionProviderKey);
                }
                else
                {
                    userLogin = await _externalUserLoginRepository.FirstOrDefaultAsync(ul => ul.LoginProvider == login.LoginProvider &&
                    ul.ProviderKey == login.ProviderKey);
                }

                if (userLogin == null)
                    return null;

                return await UserRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
            }
        }

        [UnitOfWork]
        public virtual Task<List<User>> FindAll4PlatformAsync(UserLoginInfo login)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var query = from userLogin in _externalUserLoginRepository.GetAll()
                            join user in UserRepository.GetAll() on userLogin.UserId equals user.Id
                            where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                            select user;

                return Task.FromResult(query.ToList());
            }
        }

        #endregion
    }
}