using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.Media
{
    public interface IPictureManager
    {
        IRepository<Picture, long> PictureRepository { get; }

        IQueryable<Picture> Pictures { get; }

        IRepository<PictureGroup, long> PictureGroupRepository { get; }

        IQueryable<PictureGroup> PictureGroups { get; }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="FileName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<Picture> UploadPictureAsync(byte[] fileBytes, string FileName, long groupId);

        /// <summary>
        /// 抓取url资源到云存储
        /// </summary>
        /// <param name="resURL"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<Picture> FetchPictureAsync(string resURL, long groupId);

        /// <summary>
        /// 获取图片Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetPictureUrlAsync(long id);

        /// <summary>
        /// 获取图片Url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetPictureUrl(long id);

        /// <summary>
        /// 根据id获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Picture> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Picture> GetByIdAsync(long id);

        /// <summary>
        /// 根据key获取图片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Picture> GetByKeyAsync(string key);

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="picture"></param>
        Task CreateAsync(Picture picture);

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="picture"></param>
        Task UpdateAsync(Picture picture);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture"></param>
        Task DeleteAsync(Picture picture);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        /// <summary>
        /// 根据id获取图片分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PictureGroup> GetGroupByIdAsync(long id);

        /// <summary>
        /// 根据id获取图片分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<PictureGroup> GetGroupByNameAsync(string name);

        /// <summary>
        /// 添加图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        Task CreateGroupAsync(PictureGroup pictureGroup);

        /// <summary>
        /// 更新图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        Task UpdateGroupAsync(PictureGroup pictureGroup);

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <param name="pictureGroup"></param>
        Task DeleteGroupAsync(PictureGroup pictureGroup);

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <param name="id"></param>
        Task DeleteGroupAsync(long id);
    }
}
