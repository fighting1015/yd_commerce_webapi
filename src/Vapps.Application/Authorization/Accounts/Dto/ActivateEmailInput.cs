using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;

namespace Vapps.Authorization.Accounts.Dto
{
    public class ActivateEmailInput : IShouldNormalize
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 确认码
        /// </summary>
        [Required]
        public string ConfirmationCode { get; set; }

        /// <summary>
        /// Encrypted values for {TenantId}, {UserId} and {ConfirmationCode}
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

                if (query["confirmationCode"] != null)
                {
                    ConfirmationCode = query["confirmationCode"];
                }
            }
        }
    }
}