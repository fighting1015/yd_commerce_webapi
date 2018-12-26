using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Qiniu.Http;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Common;
using Vapps.Extensions;
using Vapps.Helpers;

namespace Vapps.Providers
{
    public class QiniuStorageProvider : IStorageProvider, ITransientDependency
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IAbpSession AbpSession;
        private readonly ILogger _logger;
        private Mac mac;

        public QiniuStorageProvider(IHttpContextAccessor contextAccessor,
            IHostingEnvironment env,
            IAbpSession abpSession)
        {
            _env = env;
            _appConfiguration = _env.GetAppConfiguration();

            _contextAccessor = contextAccessor;
            AbpSession = abpSession;
            _logger = NullLogger.Instance;
        }

        /// <summary>
        /// 获取上传凭证
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public async Task<string> GetUploadImageTokenAsync(string fileKey = "")
        {
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            if (!fileKey.IsNullOrEmpty())
                putPolicy.Scope = $"{FileStorageConsts.IMAGE_BUCKET}:{fileKey}";
            else
                putPolicy.Scope = FileStorageConsts.IMAGE_BUCKET;

            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(60 * 60 * 60);

            putPolicy.CallbackUrl = $"{_appConfiguration["App:TestRootAddress"]}/api/services/app/File/UploadPictureCallBack";
            putPolicy.CallbackBody = $"{{bucket:$(bucket),name:$(fname),key:$(key),mimeType:$(mimeType),creatoruserid:{AbpSession.UserId},tenantid:{AbpSession.TenantId},groupid:$(x:groupid),imageMogr2:$(x:imageMogr2)}}";
            putPolicy.CallbackBodyType = "application/json";

            //限制文件上传类型
            putPolicy.MimeLimit = "image/jpeg;image/png;image/gif;";
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(GetMac(), jstr);

            return await Task.FromResult(token);
        }

        /// <summary>
        /// 回调鉴权
        /// </summary>
        /// <returns></returns>
        public bool VerifyCallback()
        {
            try
            {
                var authorization = _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var callBackToken = authorization.Remove(0, 5).Split(':').FirstOrDefault();

                if (callBackToken == _appConfiguration["FileStorage:Qiniu:AK"])
                    return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string fileKey, byte[] fileBytes)
        {
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = $"{FileStorageConsts.IMAGE_BUCKET}:{fileKey}";
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(60 * 60 * 60);

            putPolicy.CallbackUrl = $"{_appConfiguration["App:TestRootAddress"]}/api/services/app/File/UploadPictureCallBack";
            putPolicy.CallbackBody = $"{{name:$(fname),key:$(key),mimeType:$(mimeType),creatoruserid:{AbpSession.UserId},tenantid:{AbpSession.TenantId},groupid:0}}";
            putPolicy.CallbackBodyType = "application/json";

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(GetMac(), jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = await fu.UploadDataAsync(fileBytes, fileKey, token);

            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileWithNotCallBackAsync(string fileKey, byte[] fileBytes)
        {
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = $"{FileStorageConsts.IMAGE_BUCKET}:{fileKey}";
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(60 * 60 * 60);

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(GetMac(), jstr);
            FormUploader fu = new FormUploader();
            HttpResult result = await fu.UploadDataAsync(fileBytes, fileKey, token);

            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 先下载再上传文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileFormUrlAsync(string resURL, string fileKey)
        {
            var fileBytes = await CommonHelper.SavePictureFromUrlAsync(resURL);

            return await UploadFileWithNotCallBackAsync(fileKey, fileBytes);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldFileKey"></param>
        /// <param name="newFileKey"></param>
        /// <returns></returns>
        public async Task<bool> MoveFileAsync(string oldFileKey, string newFileKey)
        {
            BucketManager bm = new BucketManager(GetMac());
            var result = await bm.MoveAsync(FileStorageConsts.IMAGE_BUCKET, oldFileKey, FileStorageConsts.IMAGE_BUCKET, newFileKey);
            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 抓取网络资源到空间(部分源站可能会屏蔽七牛，导致抓取失败)
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> FetchAsync(string resURL, string fileKey)
        {
            BucketManager bm = new BucketManager(GetMac());
            var result = await bm.FetchAsync(resURL, FileStorageConsts.IMAGE_BUCKET, fileKey);
            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string fileKey)
        {
            BucketManager bm = new BucketManager(GetMac());

            var result = await bm.DeleteAsync(FileStorageConsts.IMAGE_BUCKET, fileKey);

            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="fileKey"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task<bool> BatchDeleteAsync(string[] fileKey)
        {
            BucketManager bm = new BucketManager(GetMac());

            var result = await bm.BatchDeleteAsync(FileStorageConsts.IMAGE_BUCKET, fileKey);

            return result.Code == (int)HttpCode.OK;
        }

        /// <summary>
        /// 空间/文件夹管理
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="delimiter"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        public async Task<StorageDirectoryInfo> GetBucketFileListAsync(string prefix, string delimiter, string skipCount, int maxResultCount)
        {
            StorageDirectoryInfo directoryInfo;
            BucketManager bm = new BucketManager(GetMac());
            ListResult result = await bm.ListAsync(FileStorageConsts.IMAGE_BUCKET, prefix, skipCount, maxResultCount, delimiter);

            if (result.Code == (int)HttpCode.OK)
            {
                directoryInfo = new StorageDirectoryInfo()
                {
                    SkipCount = result.Result.Marker,
                    CommonPrefixes = result.Result.CommonPrefixes,
                    Items = result.Result.Items.Select(x => new FileInfo()
                    {
                        FSize = x.Fsize,
                        Hash = x.Hash,
                        Key = x.Key,
                        MimeType = x.MimeType,
                        PutTime = x.PutTime
                    }).ToList()
                };
            }
            else
            {
                directoryInfo = new StorageDirectoryInfo();
            }

            return directoryInfo;
        }

        #region Utilitis

        public Mac GetMac()
        {
            if (mac == null)
                mac = new Mac(_appConfiguration["FileStorage:Qiniu:AK"], _appConfiguration["FileStorage:Qiniu:SK"]);

            return mac;
        }


        #endregion
    }
}
