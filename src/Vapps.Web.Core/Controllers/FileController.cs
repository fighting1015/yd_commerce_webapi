using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using Vapps.Dto;
using Vapps.Storage;

namespace Vapps.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FileController : VappsControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public FileController(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        /// <summary>
        /// 下载临时文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [DisableAuditing]
        [HttpGet]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }
    }
}

