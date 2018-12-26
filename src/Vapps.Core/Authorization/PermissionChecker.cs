using Abp.Authorization;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.MultiTenancy;

namespace Vapps.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
