using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Auditing;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;

namespace Vapps.Authorization.Accounts.Dto
{
    public class ResetPasswordInput : IShouldNormalize
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        /// <summary>
        /// 重置密码 Code
        /// </summary>
        [Required]
        public string ResetCode { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// Encrypted values for {TenantId}, {UserId} and {ResetCode}
        /// </summary>
        public string c { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["userId"] != null)
                {
                    UserId = Convert.ToInt32(query["userId"]);
                }

                if (query["resetCode"] != null)
                {
                    ResetCode = query["resetCode"];
                }
            }
        }
    }
}