using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Abp.Timing;

namespace Vapps.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public const string VappsTenancyNameRegex = @"([\x4E00-\x9FA5\w]{2,16})";

        /// <summary>
        /// 允许空邮箱
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(MaxEmailAddressLength)]
        public override string EmailAddress { get; set; }

        [MaxLength(MaxPhoneNumberLength)]
        public override string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(32)]
        public override string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(32)]
        public override string Surname { get; set; }


        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        /// <summary>
        /// 是否为主账户
        /// </summary>
        public virtual bool IsMainUser { get; set; }

        //Can add application specific user properties here
        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// 创建租户主账号 <see cref="User"/>
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string userName, string phoneNumber)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = userName,
                Name = userName,
                Surname = string.Empty,
                EmailAddress = emailAddress,
                IsMainUser = true,
                PhoneNumber = phoneNumber,
            };

            user.SetNormalizedNames();

            return user;
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }
    }
}