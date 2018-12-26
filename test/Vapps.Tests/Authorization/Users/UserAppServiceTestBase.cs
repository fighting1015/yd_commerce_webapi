using Abp.Authorization.Users;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using Vapps.Authorization.Users;

namespace Vapps.Tests.Authorization.Users
{
    public abstract class UserAppServiceTestBase : AppTestBase
    {
        protected readonly IUserAppService UserAppService;

        protected UserAppServiceTestBase()
        {
            UserAppService = Resolve<IUserAppService>();
        }

        protected void CreateTestUsers()
        {
            //Note: There is a default "admin" user also

            UsingDbContext(
                context =>
                {
                    context.Users.Add(CreateUserEntity("jnash", "John", "Nash", "jnsh2000@testdomain.com"));
                    context.Users.Add(CreateUserEntity("adams_d", "Douglas", "Adams", "adams_d@gmail.com"));
                    context.Users.Add(CreateUserEntity("artdent", "Arthur", "Dent", "ArthurDent@yahoo.com"));
                    context.Users.Add(CreateUserEntity("vapps", "vapps", "hk", "dev@vapps.hk", "13750013054"));
                });
        }

        protected User CreateUserEntity(string userName, string name, string surname, string emailAddress, string phoneNumber = "")
        {
            var user = new User
            {
                EmailAddress = emailAddress,
                IsEmailConfirmed = true,
                Name = name,
                Surname = surname,
                UserName = userName,
                Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==", //123qwe
                TenantId = AbpSession.TenantId,
                PhoneNumber = phoneNumber,
                Permissions = new List<UserPermissionSetting>
                {
                    new UserPermissionSetting {Name = "test.permission1", IsGranted = true, TenantId = AbpSession.TenantId},
                    new UserPermissionSetting {Name = "test.permission2", IsGranted = true, TenantId = AbpSession.TenantId},
                    new UserPermissionSetting {Name = "test.permission3", IsGranted = false, TenantId = AbpSession.TenantId},
                    new UserPermissionSetting {Name = "test.permission4", IsGranted = false, TenantId = AbpSession.TenantId}
                }
            };

            //user.SetNormalizedNames(); //TODO: use this after Abp.ZeroCore v2.1 upgrade!
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.NormalizedEmailAddress = user.EmailAddress.ToUpperInvariant();

            return user;
        }
    }
}