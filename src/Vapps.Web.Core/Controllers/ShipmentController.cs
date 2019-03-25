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
using Vapps.ECommerce.Shippings.Importing;
using Vapps.Storage;

namespace Vapps.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ShipmentController : VappsControllerBase
    {
        private readonly IBinaryObjectManager BinaryObjectManager;
        private readonly IBackgroundJobManager BackgroundJobManager;

        public ShipmentController(
            IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// 从Excel导入订单
        /// </summary>
        /// <param name="tenantLogisticsId">自选物流Id，后期改成模板Id</param>
        /// <returns></returns>
        [HttpPost]
        [AbpMvcAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Import)]
        public async Task<JsonResult> ImportFromExcel(int tenantLogisticsId)
        {
            try
            {
                var file = Request.Form.Files.First();

                if (tenantLogisticsId == 0)
                {
                    throw new UserFriendlyException(L("TenantLogisticsIsRequied"));
                }

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

                await BackgroundJobManager.EnqueueAsync<ImportShipmentsFromExcelJob, ImportShipmentsFromExcelJobArgs>(new ImportShipmentsFromExcelJobArgs
                {
                    TenantId = tenantId,
                    TenantLogisticsId = tenantLogisticsId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
