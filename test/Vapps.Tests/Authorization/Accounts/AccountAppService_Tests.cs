using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Accounts.Dto;
using Vapps.MultiTenancy;
using Shouldly;
using Xunit;

namespace Vapps.Tests.Authorization.Accounts
{
    public class AccountAppService_Tests : AppTestBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountAppService_Tests()
        {
            _accountAppService = Resolve<IAccountAppService>();
        }

        [Fact]
        public async Task Should_Check_If_Given_Tenant_Is_Available()
        {
            //Act
            var output = await _accountAppService.IsTenantAvailable(
                new IsTenantAvailableInput
                {
                    TenancyName = Tenant.DefaultTenantName
                }
            );

            //Assert
            output.State.ShouldBe(TenantAvailabilityState.Available);
            output.TenantId.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Return_NotFound_If_Tenant_Is_Not_Defined()
        {
            //Act
            var output = await _accountAppService.IsTenantAvailable(
                new IsTenantAvailableInput
                {
                    TenancyName = "UnknownTenant"
                }
            );

            //Assert
            output.State.ShouldBe(TenantAvailabilityState.NotFound);
        }

        [Fact]
        public async Task Should_Register()
        {
            //Act
            await _accountAppService.Register(new RegisterInput
            {
                PhoneNumber = "john",
                Password = "john123",
                Name = "John",
                //Surname = "Nash",
                //EmailAddress = "694978795@qq.com"
            });

            //Assert
            UsingDbContext(context =>
            {
                context.Users.FirstOrDefault(
                    u => u.TenantId == AbpSession.TenantId &&
                         u.EmailAddress == "694978795@qq.com"
                ).ShouldNotBeNull();
            });
        }
    }
}
