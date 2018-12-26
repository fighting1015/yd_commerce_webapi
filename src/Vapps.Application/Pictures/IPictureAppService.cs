using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.Pictures.Dto;

namespace Vapps.Pictures
{
    public interface IPictureAppService
    {
        #region Picture

        /// <summary>
        /// 获取分组下的图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<PictureListDto>> GetPictureAsync(GetPictureInput input);

        /// <summary>
        /// 获取当前用户上传图片凭证
        /// </summary>
        /// <returns></returns>
        Task<UploadTokenOutput> GetPictureUploadToken();

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        Task UploadAsync(long groupId);

        /// <summary>
        /// 创建或更新图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdatePicture(UpdatePictureInput input);

        /// <summary>
        /// 批量删除图片
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(BatchDeleteInput input);

        /// <summary>
        /// 批量移动图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BatchMove2Group(BatchMove2GroupInput input);

        /// <summary>
        /// 根据Url批量删除图片
        /// </summary>
        /// <returns></returns>
        Task DeleteByUrlAsync(PictureDeleteInput input);

        #endregion

        #region Picture group

        /// <summary>
        /// 获取所有图片分组
        /// </summary>
        /// <returns></returns>
        Task<List<PictureGroupListDto>> GetPictureGroupAsync();

        /// <summary>
        /// 创建或更新图片分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdatePictureGroup(CreateOrUpdatePictureGroupInput input);

        /// <summary>
        /// 删除图片分组
        /// </summary>
        /// <returns></returns>
        Task DeleteGroupAsync(EntityDto<long> input);

        #endregion
    }
}
