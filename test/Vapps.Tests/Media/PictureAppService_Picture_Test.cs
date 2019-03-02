using Abp.Application.Services.Dto;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.Media;
using Vapps.MultiTenancy;
using Vapps.Pictures.Dto;
using Xunit;

namespace Vapps.Tests.Media
{
    public class PictureAppService_Picture_Test : PictureAppServiceTestBase
    {
        [MultiTenantFact]
        public async Task Should_Create_Picture_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;

            await CreatePictureAndTestAsync("test.jpg", "1/test.jpg", 0, defaultTenantId);
        }

        [MultiTenantFact]
        public async Task Should_Update_Picture_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            await CreatePictureAndTestAsync("test.jpg", "1/test.jpg", 0, defaultTenantId);

            var picture = await GetPictureByKeyOrNullAsync("1/test.jpg");

            //Try to update with existing name
            await PictureAppService.UpdatePicture(
                new UpdatePictureInput
                {
                    Id = picture.Id,
                    Name = "test2.jpg",
                });

            //Assert
            var pictureAfterUpdate = await GetPictureByKeyOrNullAsync("1/test.jpg");
            pictureAfterUpdate.Name.ShouldBe("test2.jpg");
        }

        [MultiTenantFact]
        public async Task Should_Delete_Picture_For_Tenant()
        {
            LoginAsDefaultTenantAdmin();

            var defaultTenantId = (await GetTenantAsync(Tenant.DefaultTenantName)).Id;
            await CreatePictureAndTestAsync("test.jpg", "1/test.jpg", 0, defaultTenantId);

            var picture = await GetPictureByKeyOrNullAsync("1/test.jpg");

            await PictureAppService.DeleteAsync(new BatchInput<long>()
            {
                Ids = new long[] { picture.Id }
            });

            //Assert
            var pictureDeleted = await GetPictureByKeyOrNullAsync("1/test.jpg");
            pictureDeleted.ShouldBeNull();
        }


        private async Task<Picture> GetPictureByKeyOrNullAsync(string key)
        {
            return await UsingDbContextAsync(async context =>
               await context.Pictures
                   .FirstOrDefaultAsync(u =>
                           u.Key == key &&
                           u.TenantId == AbpSession.TenantId
                   ));
        }

        private async Task CreatePictureAndTestAsync(string name, string key, int groupId, int tenantId)
        {
            //Arrange
            AbpSession.TenantId = tenantId;

            //Act
            await PictureAppService.UpdatePicture(
                new UpdatePictureInput
                {
                    Name = name,
                    GroupId = groupId,
                });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                //Get created user
                var picture = await context.Pictures.FirstOrDefaultAsync(u => u.Key == key);
                picture.ShouldNotBe(null);

                //Check some properties
                picture.Name.ShouldBe(name);
                picture.TenantId.ShouldBe(tenantId);
                picture.CreatorUserId.ShouldBe(GetCurrentUser().Id);
            });
        }
    }
}
