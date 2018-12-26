using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Vapps.Authorization.Users;
using Vapps.MultiTenancy;

namespace Vapps.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}