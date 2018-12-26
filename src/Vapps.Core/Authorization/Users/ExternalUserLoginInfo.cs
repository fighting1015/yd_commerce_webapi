using Microsoft.AspNetCore.Identity;

namespace Vapps.Authorization.Users
{
    public class ExternalUserLoginInfo : UserLoginInfo
    {
        /// <summary>
        /// 同平台下唯一凭证
        /// </summary>
        public virtual string UnionProviderKey { get; set; }

        public ExternalUserLoginInfo(string loginProvider, string unifiedProviderKey, string providerKey, string displayName)
            : base(loginProvider, providerKey, displayName)
        {
            this.UnionProviderKey = unifiedProviderKey;
        }
    }
}
