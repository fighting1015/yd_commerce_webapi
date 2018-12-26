
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Vapps.Authorization;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.Editions;
using Vapps.MultiTenancy;

namespace Vapps.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
            {
            }).AddAbpTenantManager<TenantManager>()
              .AddAbpUserManager<UserManager>()
              .AddAbpRoleManager<RoleManager>()
              .AddAbpEditionManager<EditionManager>()
              .AddAbpUserStore<UserStore>()
              .AddAbpRoleStore<RoleStore>()
              .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
              .AddAbpSecurityStampValidator<SecurityStampValidator>()
              .AddPermissionChecker<PermissionChecker>()
              .AddAbpSignInManager<SignInManager>()
              .AddDefaultTokenProviders();
        }
    }
}
