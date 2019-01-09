using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Extensions;
using Vapps.Media.Cache;

namespace Vapps.Media
{
    public class PictureManager : VappsDomainServiceBase, IPictureManager
    {
        public IRepository<Picture, long> PictureRepository { get; }
        public IQueryable<Picture> Pictures => PictureRepository.GetAll().AsNoTracking();
        public IRepository<PictureGroup, long> PictureGroupRepository { get; }
        public IQueryable<PictureGroup> PictureGroups => PictureGroupRepository.GetAll().AsNoTracking();

        private readonly IStorageProvider _storageProvider;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IPictureCache _pictureCache;

        public PictureManager(IRepository<Picture, long> pictureRepository,
            IRepository<PictureGroup, long> pictureGroupRepository,
            IAbpSession abpSession,
            IUnitOfWorkManager unitOfWorkManager,
            IStorageProvider storageProvider,
            IPictureCache pictureCache)
        {
            this.PictureRepository = pictureRepository;
            this.PictureGroupRepository = pictureGroupRepository;
            this._abpSession = abpSession;
            this._unitOfWorkManager = unitOfWorkManager;
            this._storageProvider = storageProvider;
            this._pictureCache = pictureCache;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="FileName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<Picture> UploadPictureAsync(byte[] fileBytes, string FileName, long groupId)
        {
            var fileKey = GeneratePictureKey(Guid.NewGuid().ToLongId().ToString(), groupId);

            //上传到云存储
            await _storageProvider.UploadFileAsync(fileKey, fileBytes);

            var picture = new Picture()
            {
                Name = FileName,
                Key = fileKey,
                GroupId = groupId,
                OriginalUrl = $"{FileStorageDomainHelper.GetImgBucketDomain()}/{fileKey}",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _abpSession.UserId,
                TenantId = GetCurrentTenantId(),
            };

            await CreateAsync(picture);

            _unitOfWorkManager.Current.SaveChanges();

            return picture;
        }

        /// <summary>
        /// 抓取url资源到云存储
        /// </summary>
        /// <param name="resURL"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<Picture> FetchPictureAsync(string resURL, long groupId)
        {
            var fileKey = GeneratePictureKey(Guid.NewGuid().ToLongId().ToString(), groupId);

            //上传到云存储
            var result = await _storageProvider.UploadFileFormUrlAsync(resURL, fileKey);
            if (!result) return null; // 抓取失败，返回空对象

            var picture = new Picture()
            {
                Name = fileKey,
                Key = fileKey,
                GroupId = groupId,
                OriginalUrl = $"{FileStorageDomainHelper.GetImgBucketDomain()}/{fileKey}",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _abpSession.UserId,
                TenantId = GetCurrentTenantId(),
            };

            await CreateAsync(picture);

            _unitOfWorkManager.Current.SaveChanges();

            return picture;
        }

        /// <summary>
        /// 获取图片Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<string> GetPictureUrlAsync(long id)
        {
            if (id <= 0)
                return string.Empty;

            var picture = await _pictureCache.GetAsync(id);
            if (picture == null)
                return string.Empty;

            return picture.OriginalUrl;
        }

        /// <summary>
        /// 获取图片Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetPictureUrl(long id)
        {
            if (id <= 0)
                return string.Empty;

            var picture = _pictureCache.Get(id);
            if (picture == null)
                return string.Empty;

            return picture.OriginalUrl;
        }

        /// <summary>
        /// 根据id获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Picture> FindByIdAsync(long id)
        {
            return await PictureRepository.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// 根据id获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Picture> GetByIdAsync(long id)
        {
            return await PictureRepository.GetAsync(id);
        }

        /// <summary>
        /// 根据key获取图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<Picture> GetByKeyAsync(string key)
        {
            return await PictureRepository.FirstOrDefaultAsync(p => p.Key == key);
        }


        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="picture"></param>
        public virtual async Task CreateAsync(Picture picture)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !picture.TenantId.HasValue)
            {
                picture.TenantId = tenantId.Value;
            }

            await PictureRepository.InsertAsync(picture);
        }

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="picture"></param>
        public virtual async Task UpdateAsync(Picture picture)
        {
            await PictureRepository.UpdateAsync(picture);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture"></param>
        public virtual async Task DeleteAsync(Picture picture)
        {
            await PictureRepository.DeleteAsync(picture);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var picture = await PictureRepository.FirstOrDefaultAsync(id);

            if (picture != null)
                await PictureRepository.DeleteAsync(picture);
        }

        /// <summary>
        /// 根据id获取图片分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PictureGroup> GetGroupByIdAsync(long id)
        {
            var defaultGroup = EnumExtensions.EnumToSelectListItem(DefaultGroups.All, VappsConsts.ServerSideLocalizationSourceName);
            var result = defaultGroup.FirstOrDefault(g => g.Value == id.ToString());
            if (result != null)
            {
                return new PictureGroup()
                {
                    Id = id,
                    Name = result.Text,
                    IsSystemGroup = true
                };
            }

            return await PictureGroupRepository.GetAsync(id);
        }

        /// <summary>
        /// 根据名称获取图片分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<PictureGroup> GetGroupByNameAsync(string name)
        {
            var defaultGroup = EnumExtensions.EnumToSelectListItem(DefaultGroups.All, VappsConsts.ServerSideLocalizationSourceName);
            var result = defaultGroup.FirstOrDefault(g => g.Text == name);
            if (result != null)
            {
                return new PictureGroup()
                {
                    Id = Convert.ToInt32(result.Value),
                    Name = result.Text,
                    IsSystemGroup = true
                };
            }

            return await PictureGroupRepository.FirstOrDefaultAsync(pg => pg.Name == name);
        }

        /// <summary>
        /// 添加图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        public virtual async Task CreateGroupAsync(PictureGroup pictureGroup)
        {
            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !pictureGroup.TenantId.HasValue)
            {
                pictureGroup.TenantId = tenantId.Value;
            }

            await PictureGroupRepository.InsertAsync(pictureGroup);
        }

        /// <summary>
        /// 更新图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        public virtual async Task UpdateGroupAsync(PictureGroup pictureGroup)
        {
            await PictureGroupRepository.InsertOrUpdateAsync(pictureGroup);
        }

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        public virtual async Task DeleteGroupAsync(PictureGroup pictureGroup)
        {
            await PictureGroupRepository.DeleteAsync(pictureGroup);
        }

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteGroupAsync(long id)
        {
            var pictureGroup = await PictureGroupRepository.FirstOrDefaultAsync(id);

            if (pictureGroup != null)
                await PictureGroupRepository.DeleteAsync(pictureGroup);
        }

        /// <summary>
        /// 生成唯一的key
        /// </summary>
        /// <param name="name"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public virtual string GeneratePictureKey(string name, long groupId)
        {
            if (CurrentUnitOfWork.GetTenantId().HasValue)
            {
                return $"{CurrentUnitOfWork.GetTenantId().Value}/{groupId}/{name}";
            }

            return $"0/{groupId}/{name}";
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return _abpSession.TenantId;
        }
    }
}
