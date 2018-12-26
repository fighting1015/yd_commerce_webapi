using Abp.Runtime.Session;
using System.Linq;

namespace Vapps.Runtime
{
    /// <summary>
    /// 通过扩展方法来对AbpSession进行扩展
    /// </summary>
    public static class SessionExtension
    {
        public const string AccessIds = "Vapps.AccessIds";

        /// <summary>
        /// 获取cookies中已访问过的Id
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetAccessIds(this IAbpSession session)
        {
            return GetClaimValue(AccessIds);
        }

        private static string GetClaimValue(string claimType)
        {
            var claimsPrincipal = DefaultPrincipalAccessor.Instance.Principal;

            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }
    }
}
