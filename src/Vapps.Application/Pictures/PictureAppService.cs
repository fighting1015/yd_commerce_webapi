using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.Helpers;
using Vapps.Media;
using Vapps.Pictures.Dto;

namespace Vapps.Pictures
{
    [AbpAuthorize]
    public class PictureAppService : BusinessCenterAppServiceBase, IPictureAppService
    {
        private readonly IPictureManager _pictureManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IStorageProvider _storageProvider;
        private readonly IAppFolders _appFolders;
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public PictureAppService(IPictureManager pictureManager,
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

        #region Picture

        /// <summary>
        /// 获取分组下的图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PictureListDto>> GetPictureAsync(GetPictureInput input)
        {
            var query = _pictureManager.Pictures
                .WhereIf(input.GroupId > -1, p => p.GroupId == input.GroupId);

            var pictureCount = await query.CountAsync();
            var picture = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var pictureListDtos = ObjectMapper.Map<List<PictureListDto>>(picture);

            return new PagedResultDto<PictureListDto>(
                pictureCount,
                pictureListDtos);
        }

        /// <summary>
        /// 获取当前用户上传图片凭证
        /// </summary>
        /// <returns></returns>
        public async Task<UploadTokenOutput> GetPictureUploadToken()
        {
            var token = await _storageProvider.GetUploadImageTokenAsync();

            return new UploadTokenOutput()
            {
                Token = token,
                ExpirationOnUtc = DateTime.UtcNow.AddSeconds(60 * 60 * 60)
            };
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public async Task UploadAsync(long groupId)
        {
            var profilePictureFile = _contextAccessor.HttpContext.Request.Form.Files.First();

            //Check input
            if (profilePictureFile == null)
            {
                throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
            }

            if (profilePictureFile.Length > 1048576) //1MB.
            {
                throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
            }

            byte[] fileBytes;
            using (var stream = profilePictureFile.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
            {
                throw new Exception("Uploaded file is not an accepted image file !");
            }

            await _pictureManager.UploadPictureAsync(fileBytes, profilePictureFile.FileName, groupId);
        }

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdatePicture(UpdatePictureInput input)
        {
            await UpdateAsync(input);
        }

        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync(BatchDeleteInput<long> input)
        {
            foreach (var id in input.Ids)
            {
                var picture = await _pictureManager.GetByIdAsync(id);

                //删除云存储资源文件
                await _storageProvider.DeleteAsync(picture.Key);

                await _pictureManager.DeleteAsync(picture);
            }
        }

        /// <summary>
        /// 批量移动图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchMove2Group(BatchMove2GroupInput input)
        {
            var group = await _pictureManager.GetGroupByIdAsync(input.GroupId);

            var pictures = await _pictureManager.PictureRepository.GetAll().Where(p => input.Ids.Contains(p.Id)).ToListAsync();

            if (pictures.Any())
            {
                pictures.Each(p => p.GroupId = input.GroupId);
            }
        }

        /// <summary>
        /// 根据Url批量删除图片
        /// </summary>
        /// <returns></returns>
        public async Task DeleteByUrlAsync(PictureDeleteInput input)
        {
            var pictures2Delete = await _pictureManager.Pictures
                .WhereIf(input.Urls.Any(), p => input.Urls.Contains(p.OriginalUrl)).ToListAsync();

            foreach (var picture in pictures2Delete)
            {
                //删除云存储资源文件
                await _storageProvider.DeleteAsync(picture.Key);

                await _pictureManager.DeleteAsync(picture.Id);
            }
        }

        #endregion

        #region Picture group

        /// <summary>
        /// 获取所有的图片分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<PictureGroupListDto>> GetPictureGroupAsync()
        {
            int pictureCount = 0;
            List<PictureGroupListDto> groupListDtos = new List<PictureGroupListDto>();

            var defaultGroups = EnumExtensions.EnumToSelectListItem(DefaultGroups.All, VappsConsts.ServerSideLocalizationSourceName);
            foreach (var item in defaultGroups)
            {
                int groupId = int.Parse(item.Value);
                var groupDto = new PictureGroupListDto()
                {
                    Id = groupId,
                    Name = item.Text,
                    IsSystemGroup = true,
                };

                if (groupId != (int)DefaultGroups.All)
                {
                    groupDto.PictureNum = await _pictureManager.Pictures.Where(p => p.GroupId == groupId).CountAsync();
                    pictureCount += groupDto.PictureNum;
                }
                groupListDtos.Add(groupDto);
            }

            var groups = await _pictureManager.PictureGroups.OrderByDescending(o => o.Id).ToListAsync();
            foreach (var item in groups)
            {
                var groupDto = new PictureGroupListDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationTime = item.CreationTime,
                    CreatorUserId = item.CreatorUserId,
                    PictureNum = await _pictureManager.Pictures.Where(p => p.GroupId == item.Id).CountAsync(),
                };
                groupListDtos.Add(groupDto);
                pictureCount += groupDto.PictureNum;
            }

            groupListDtos.FirstOrDefault().PictureNum = pictureCount;

            return groupListDtos;
        }


        /// <summary>
        /// 创建或更新图片分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdatePictureGroup(CreateOrUpdatePictureGroupInput input)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                await UpdateGroupAsync(input);
            }
            else
            {
                await CreateGroupAsync(input);
            }
        }

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <returns></returns>
        public async Task DeleteGroupAsync(EntityDto<long> input)
        {
            var isSystemGroup = Enum.IsDefined(typeof(DefaultGroups), (int)input.Id);

            if (isSystemGroup)
                throw new UserFriendlyException("PictureGallery.Group.SystemGroup.CantNotDelete");

            var pictureGroup = await _pictureManager.GetGroupByIdAsync(input.Id);

            //无法删除系统分组
            if (pictureGroup.IsSystemGroup)
                throw new UserFriendlyException("PictureGallery.Group.SystemGroup.CantNotDelete");

            var pictures = await _pictureManager.Pictures.Where(p => p.GroupId == pictureGroup.Id).ToListAsync();
            pictures.Each(t => t.GroupId = 0);

            await _pictureManager.DeleteGroupAsync(pictureGroup);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <returns></returns>
        protected async Task UpdateAsync(UpdatePictureInput input)
        {
            var picture = await _pictureManager.GetByIdAsync(input.Id);

            picture.Name = input.Name;
            if (input.GroupId.HasValue && input.GroupId != picture.GroupId)
            {
                //string newKey = GeneratePictureKey(input.Name, input.GroupId.Value);
                //await _storageProvider.MoveFileAsync(picture.Key, newKey);

                picture.GroupId = input.GroupId.Value;
                //picture.Key = newKey;
            }

            await _pictureManager.UpdateAsync(picture);
        }

        /// <summary>
        /// 生成唯一的key
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        protected string GeneratePictureKey(string name, long groupId)
        {
            if (CurrentUnitOfWork.GetTenantId().HasValue)
            {
                return $"{CurrentUnitOfWork.GetTenantId().Value}/{groupId}/{name}";
            }

            return string.Empty;
        }

        /// <summary>
        /// 创建图片分组
        /// </summary>
        /// <returns></returns>
        protected async Task CreateGroupAsync(CreateOrUpdatePictureGroupInput input)
        {
            var existedGroup = await _pictureManager.GetGroupByNameAsync(input.Name);

            if (existedGroup != null)
                throw new UserFriendlyException(string.Format(L("PictureGallery.Group.Existed"), input.Name));

            var pictureGroup = new PictureGroup()
            {
                Name = input.Name,
            };

            await _pictureManager.CreateGroupAsync(pictureGroup);
        }

        /// <summary>
        /// 更新图片分组
        /// </summary>
        /// <returns></returns>
        protected async Task UpdateGroupAsync(CreateOrUpdatePictureGroupInput input)
        {
            var pictureGroup = await _pictureManager.GetGroupByIdAsync(input.Id.Value);

            if (pictureGroup.IsSystemGroup)
                throw new UserFriendlyException(L("PictureGallery.Group.SystemGroup.CantNotUpdate"));

            var existedGroup = await _pictureManager.GetGroupByNameAsync(input.Name);
            if (existedGroup != null && pictureGroup.Id != existedGroup.Id)
                throw new UserFriendlyException(string.Format(L("PictureGallery.Group.Existed"), input.Name));

            pictureGroup.Name = input.Name;

            await _pictureManager.UpdateGroupAsync(pictureGroup);
        }

        #endregion
    }
}
