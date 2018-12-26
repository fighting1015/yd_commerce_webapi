using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Files.Dto;

namespace Vapps.Files
{
    public interface IFileAppService
    {
        /// <summary>
        /// 云存储回调
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UploadPictureOutput> UploadPictureCallBack(UploadPictureInput input);
    }
}
