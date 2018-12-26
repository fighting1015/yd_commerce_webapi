using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using Vapps.Authorization;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.EntityFrameworkCore;
using Vapps.MultiTenancy;

namespace Vapps.Tests.TestDatas
{
    /* Creates Test tentant and User 
  
    */
    public class TestTenantAndUserBuilder
    {
        private readonly VappsDbContext _context;

        public TestTenantAndUserBuilder(VappsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateTUs();
        }

        private void CreateTUs()
        {
            CreateRolesAndUsers("vapps", "vapps");
        }

        private void CreateRolesAndUsers(string tenantName, string userName)
        {
            var tenant = _context.Tenants.Add(new Tenant(tenantName, tenantName)).Entity;
            _context.SaveChanges();

            //Admin role
            var adminRole = _context.Roles.FirstOrDefault(r => r.TenantId == tenant.Id && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(tenant.Id, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();

                //Grant all permissions to admin role
                var permissions = PermissionFinder
                    .GetAllPermissions(new AppAuthorizationProvider(false))
                    .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant))
                    .ToList();

                foreach (var permission in permissions)
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting
                        {
                            TenantId = tenant.Id,
                            Name = permission.Name,
                            IsGranted = true,
                            RoleId = adminRole.Id
                        });
                }

                _context.SaveChanges();
            }

            //User role
            var userRole = _context.Roles.FirstOrDefault(r => r.TenantId == tenant.Id && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(tenant.Id, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            //admin user
            var adminUser = _context.Users.FirstOrDefault(u => u.TenantId == tenant.Id && u.UserName == userName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(tenant.Id, "", userName, string.Empty);
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(tenant.Id, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                //_context.Accounts.Add(new Account
                //{
                //    TenantId = tenant.Id,
                //    UserId = adminUser.Id,
                //    UserName = userName,
                //    EmailAddress = adminUser.EmailAddress,
                //});
                _context.SaveChanges();
            }
        }
    }
}
