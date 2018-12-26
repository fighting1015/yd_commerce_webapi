using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vapps.MultiTenancy;
using Vapps.Pictures.Dto;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Vapps.Media;
using Abp.Application.Services.Dto;
using Xunit;
using Abp.UI;

namespace Vapps.Tests.Media
{
    public class PictureAppService_PictureGroup_Test : PictureAppServiceTestBase
    {
        [MultiTenantFact]
        public async Task Should_Create_PictureGroup_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;

            await CreatePictureGroupAndTestAsync("测试分组", defaultTenantId);
        }

        [MultiTenantFact]
        public async Task Should_Update_PictureGroup_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;

            await CreatePictureGroupAndTestAsync("测试分组", defaultTenantId);

            var existGroup = await GetPictureGroupByNameOrNullAsync("测试分组");

            //Try to update with existing name
            await PictureAppService.CreateOrUpdatePictureGroup(
                new CreateOrUpdatePictureGroupInput
                {
                    Id = existGroup.Id,
                    Name = "测试分组更新",
                });

            //Assert
            var picturegroupAfterUpdate = await GetPictureGroupByNameOrNullAsync("测试分组更新");
            picturegroupAfterUpdate.Name.ShouldBe("测试分组更新");
        }

        [MultiTenantFact]
        public async Task Should_Delete_PictureGroup_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;

            //Try to delete system group
            var friendlyException = await Assert.ThrowsAsync<UserFriendlyException>(async () =>
            await PictureAppService.DeleteGroupAsync(new EntityDto<long>(1)));

            await CreatePictureGroupAndTestAsync("测试分组", defaultTenantId);
            var existGroup = await GetPictureGroupByNameOrNullAsync("测试分组");
            await PictureAppService.DeleteGroupAsync(new EntityDto<long>(existGroup.Id));
        }

        private async Task<PictureGroup> GetPictureGroupByNameOrNullAsync(string name)
        {
            return await UsingDbContextAsync(async context =>
               await context.PictureGroups
                   .FirstOrDefaultAsync(u =>
                           u.Name == name &&
                           u.TenantId == AbpSession.TenantId
                   ));
        }

        private async Task CreatePictureGroupAndTestAsync(string name, int tenantId)
        {
            //Arrange
            AbpSession.TenantId = tenantId;

            //Act
            await PictureAppService.CreateOrUpdatePictureGroup(
                new CreateOrUpdatePictureGroupInput
                {
                    Name = name,
                });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                //Get created user
                var pictureGroup = await context.PictureGroups.FirstOrDefaultAsync(u => u.Name == name);
                pictureGroup.ShouldNotBe(null);

                //Check some properties
                pictureGroup.Name.ShouldBe(name);
                pictureGroup.TenantId.ShouldBe(tenantId);
                pictureGroup.CreatorUserId.ShouldBe(GetCurrentUser().Id);
            });
        }
    }
}
