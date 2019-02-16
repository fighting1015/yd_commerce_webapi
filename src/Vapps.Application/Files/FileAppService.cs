using Abp.Configuration.Startup;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Vapps.Files.Dto;
using Vapps.Media;

namespace Vapps.Files
{
    public class FileAppService : VappsAppServiceBase, IFileAppService
    {
        private readonly IPictureManager _pictureManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IStorageProvider _storageProvider;
        private readonly IAppFolders _appFolders;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public FileAppService(IPictureManager pictureManager,
               IStorageProvider storageProvider,
               IAppFolders appFolders,
               IHttpContextAccessor contextAccessor,
               IMultiTenancyConfig multiTenancyConfig)
        {
            this._pictureManager = pictureManager;
            this._storageProvider = storageProvider;
            this._contextAccessor = contextAccessor;
            this._appFolders = appFolders;
            this._multiTenancyConfig = multiTenancyConfig;
        }

        /// <summary>
        /// 云存储回调
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<UploadPictureOutput> UploadPictureCallBack(UploadPictureInput input)
        {
            if (!_storageProvider.VerifyCallback())
                throw new UserFriendlyException("File.UploadPictureCallBack.InvalidRequest");

            var picture = await _pictureManager.GetByKeyAsync(input.Key);
            if (picture == null)
            {
                var originalUrl = $"{GetBucketDomain(input.Bucket)}/{input.Key}";
                if (!input.ImageMogr2.IsNullOrEmpty())
                    originalUrl = $"{originalUrl}?{input.ImageMogr2}";

                picture = new Picture()
                {
                    Name = input.Name,
                    Key = input.Key,
                    GroupId = input.GroupId,
                    OriginalUrl = originalUrl,
                    CreationTime = DateTime.UtcNow,
                    CreatorUserId = input.CreatorUserId,
                    TenantId = input.TenantId,
                };

                if (input.CreatorUserId > 0)
                    picture.CreatorUserId = input.CreatorUserId;

                await _pictureManager.CreateAsync(picture);
            }
            else
            {
                picture.Name = input.Name;
                picture.Key = input.Key;
                picture.GroupId = input.GroupId;
                picture.OriginalUrl = $"{GetBucketDomain(input.Bucket)}/{input.Key}";
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return new UploadPictureOutput()
            {
                Id = picture.Id,
                Url = picture.OriginalUrl
            };
        }

        /// <summary>
        /// 获取存储空间域名
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        private string GetBucketDomain(string bucket)
        {
            FileStorageConsts.BUCKET_DOMAIN.TryGetValue(bucket, out string domain);

            return domain;
        }
    }
}
