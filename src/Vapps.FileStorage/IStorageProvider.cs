using System.IO;
using System.Threading.Tasks;
using Vapps.Common;

namespace Vapps
{
    public interface IStorageProvider
    {
        /// <summary>
        /// 获取上传凭证
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        Task<string> GetUploadImageTokenAsync(string fileKey = "");

        /// <summary>
        /// 回调鉴权
        /// </summary>
        /// <returns></returns>
        bool VerifyCallback();

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        Task<bool> UploadFileAsync(string fileKey, byte[] fileBytes);

        /// <summary>
        /// 先下载再上传文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        Task<bool> UploadFileFormUrlAsync(string resURL, string fileKey);

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldFileKey"></param>
        /// <param name="newFileKey"></param>
        /// <returns></returns>
        Task<bool> MoveFileAsync(string oldFileKey, string newFileKey);

        /// <summary>
        /// 抓取网络资源到空间
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        Task<bool> FetchAsync(string resURL, string fileKey);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string fileKey);

        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        Task<bool> BatchDeleteAsync(string[] fileKey);

        /// <summary>
        /// 获取 空间/文件夹下的文件
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="delimiter"></param>
        /// <param name="skipCount">此处使用字符串</param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<StorageDirectoryInfo> GetBucketFileListAsync(string prefix, string delimiter, string skipCount, int maxResultCount);
    }
}
