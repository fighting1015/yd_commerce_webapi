using Abp.Localization;
using Abp.Zero;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vapps.Authorization
{
    /// <summary>
    /// 账号验证类
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class VappsUserValidator<TUser> : UserValidator<TUser>
        where TUser : class
    {
        public readonly bool AllowOnlyAlphanumericUserNames;
        public readonly bool RequireUniqueEmail;
        public readonly ILocalizationManager _localizationManager;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="manager"></param>
        public VappsUserValidator(ILocalizationManager localizationManager) : base()
        {
            this._localizationManager = localizationManager;
            this.AllowOnlyAlphanumericUserNames = false;
            this.RequireUniqueEmail = false;
        }

        /// <summary>
        /// 验证用户有效性 <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the validation operation.</returns>
        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var errors = new List<IdentityError>();
            await ValidateUserName(manager, user, errors);
            if (this.RequireUniqueEmail)
            {
                await ValidateEmail(manager, user, errors);
            }
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        /// <summary>
        /// 检查用户名有效性
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private async Task ValidateUserName(UserManager<TUser> manager, TUser user, ICollection<IdentityError> errors)
        {
            var userName = await manager.GetUserNameAsync(user);
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(new IdentityError { Code = "InvalidUserName", Description = L("Identity.RequiredName") });
            }
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(userName, "^[A-Za-z0-9@_\\.]+$"))
            {
                errors.Add(new IdentityError { Code = "InvalidUserName", Description = L("Identity.InvaildName") });
            }
            else
            {
                var owner = await manager.FindByNameAsync(userName);
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError { Code = "DuplicateUserName", Description = string.Format(L("Identity.DuplicateName"), userName) });
                }
            }
        }

        /// <summary>
        /// 检查邮箱有效性
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private async Task ValidateEmail(UserManager<TUser> manager, TUser user, List<IdentityError> errors)
        {
            var email = await manager.GetEmailAsync(user);
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(new IdentityError { Code = "InvalidEmail", Description = L("Identity.RequiredEmail") });
                return;
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                errors.Add(new IdentityError { Code = "InvalidEmail", Description = L("Identity.InvalidEmail") });
                return;
            }
            var owner = await manager.FindByEmailAsync(email);
            if (owner != null &&
                !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
            {
                errors.Add(new IdentityError { Code = "DuplicateEmail", Description = string.Format(L("Identity.DuplicateEmail"), email) });
            }
        }

        private string L(string name)
        {
            return _localizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }
    }

}
