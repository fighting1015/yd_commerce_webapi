using System;
using System.Collections.Generic;
using System.Text;
using Vapps.Media;
using Vapps.Pictures;

namespace Vapps.Tests.Media
{
    public abstract class PictureAppServiceTestBase : AppTestBase
    {
        protected readonly IPictureAppService PictureAppService;

        protected PictureAppServiceTestBase() : base()
        {
            PictureAppService = Resolve<IPictureAppService>();
        }

        protected void CreatePictureAndGroup()
        {

            UsingDbContext(
                context =>
                {
                    context.PictureGroups.Add(CreatePictureGroupEntity("预约"));
                    context.PictureGroups.Add(CreatePictureGroupEntity("门店"));
                    context.PictureGroups.Add(CreatePictureGroupEntity("联系人"));
                    context.PictureGroups.Add(CreatePictureGroupEntity("头像"));

                    context.Pictures.Add(CreatePictureEntity("1.jpg", 0, "1.jpg", "jpeg"));
                    context.Pictures.Add(CreatePictureEntity("2.png", 1, "2.png", "png"));
                    context.Pictures.Add(CreatePictureEntity("3.gif", 2, "3.gif", "gif"));
                    context.Pictures.Add(CreatePictureEntity("4.jpeg", 3, "4.jpeg", "jpeg"));
                });
        }

        protected PictureGroup CreatePictureGroupEntity(string name)
        {
            return new PictureGroup
            {
                Name = name,
            };
        }

        protected Picture CreatePictureEntity(string name, int groupId, string key, string mimeType = "", string originalUrl = "")
        {
            return new Picture
            {
                Name = name,
                GroupId = groupId,
                Key = key,
                MimeType = mimeType,
                OriginalUrl = originalUrl,
                TenantId = GetCurrentTenant().Id,
            };
        }
    }
}
