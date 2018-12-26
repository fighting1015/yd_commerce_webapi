using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Helpers;
using Vapps.Media;
using Vapps.MultiTenancy;
using Vapps.Storage;

namespace Vapps.Web.Controllers
{
    /// <summary>
    /// 租户自定义配置
    /// </summary>
    [AbpMvcAuthorize]
    public class TenantCustomizationController : VappsControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IPictureManager _pictureManager;

        public TenantCustomizationController(
            IAppFolders appFolders,
            TenantManager tenantManager,
            IBinaryObjectManager binaryObjectManager,
            IPictureManager pictureManager)
        {
            _tenantManager = tenantManager;
            _binaryObjectManager = binaryObjectManager;
            _pictureManager = pictureManager;
        }

        /// <summary>
        /// 上传Logo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AbpMvcAuthorize(AdminPermissions.Configuration.TenantSettings)]
        public async Task<JsonResult> UploadLogo()
        {
            try
            {
                var logoFile = Request.Form.Files.First();

                //Check input
                if (logoFile == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (logoFile.Length > 30720) //30KB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = logoFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var imageFormat = ImageFormatHelper.GetRawImageFormat(fileBytes);
                if (!imageFormat.IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new UserFriendlyException("File_Invalid_Type_Error");
                }

                //var logoObject = new BinaryObject(AbpSession.GetTenantId(), fileBytes);
                //await _binaryObjectManager.SaveAsync(logoObject);


                var logo = await _pictureManager.UploadPictureAsync(fileBytes, logoFile.FileName, (int)DefaultGroups.ProfilePicture);
                var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
                //tenant.LogoId = logo.Id;
                //tenant.LogoFileType = logoFile.ContentType;

                return Json(new AjaxResponse(new { id = logo.Id }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        /// <summary>
        /// 获取自定义Logo
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [AbpMvcAuthorize(AdminPermissions.Configuration.TenantSettings)]
        public async Task<ActionResult> GetLogo()
        {
            var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            if (!tenant.HasLogo())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            var picture = await _pictureManager.GetByIdAsync(tenant.LogoId);
            var logo = await _pictureManager.GetPictureUrlAsync(tenant.LogoId);
            if (logo.IsNullOrEmpty())
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(await CommonHelper.SavePictureFromUrlAsync(picture.OriginalUrl), picture.MimeType);
        }
    }
}