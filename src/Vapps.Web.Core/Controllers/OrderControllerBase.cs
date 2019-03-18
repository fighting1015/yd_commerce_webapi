using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Storage;

namespace Vapps.Web.Controllers
{
    public abstract class OrderControllerBase : VappsControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;

        protected OrderControllerBase(
            IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// 从Excel导入订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AbpMvcAuthorize(BusinessCenterPermissions.Order.Import)]
        public async Task<JsonResult> ImportFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                //await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
