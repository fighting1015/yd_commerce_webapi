using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Shippings.Importing.Dto;
using Vapps.Notifications;
using Vapps.Storage;
using Hangfire;
using System.ComponentModel;

namespace Vapps.ECommerce.Shippings.Importing
{
    public class ImportShipmentsFromExcelJob : BackgroundJob<ImportShipmentsFromExcelJobArgs>, ITransientDependency
    {
        private readonly IShipmentListExcelDataReader _shipmentListExcelDataReader;
        private readonly IInvalidShipmentExporter _invalidUserExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IOrderManager _orderManager;
        private readonly IOrderImportor _orderImportor;
        private readonly IOrderProcessingManager _orderProcessingManager;
        private readonly ILogisticsManager _logisticsManager;

        public ImportShipmentsFromExcelJob(
            IShipmentListExcelDataReader shippmentListExcelDataReader,
            IInvalidShipmentExporter invalidUserExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IOrderManager orderManager,
            IOrderImportor orderImportor,
            IOrderProcessingManager orderProcessingManager,
            ILogisticsManager logisticsManager)
        {
            _shipmentListExcelDataReader = shippmentListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _orderImportor = orderImportor;
            _orderManager = orderManager;
            _orderProcessingManager = orderProcessingManager;
            _logisticsManager = logisticsManager;
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
            _localizationSource = localizationManager.GetSource(VappsConsts.ServerSideLocalizationSourceName);
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("物流单号导入任务, 租户id:{0}")]
        [Queue("shipment")]
        [UnitOfWork]
        public override void Execute(ImportShipmentsFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var shipments = GetShipmentListFromExcelOrNull(args);
                if (shipments == null || !shipments.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                AsyncHelper.RunSync(() => CreateShipments(args, shipments));
            }
        }


        private async Task CreateShipments(ImportShipmentsFromExcelJobArgs args, List<ImportShipmentDto> shipmentDtos)
        {
            var invalidShipments = new List<ImportShipmentDto>();

            var logistics = await _logisticsManager.GetTenantLogisticsByIdAsync(args.TenantLogisticsId);

            foreach (var shipmentDto in shipmentDtos)
            {

                if (shipmentDto.CanBeImported())
                {
                    var order = await _orderManager.FindByOrderNumberAsync(shipmentDto.OrderNumber);
                    if (order == null)
                    {
                        shipmentDto.Exception = "找不到订单";
                        continue;
                    }
                    try
                    {
                        var shipment = await _orderImportor.AddShipment(new OrderImport()
                        {
                            OrderStatus = OrderStatus.Processing,
                            TrackingNumber = shipmentDto.LogisticsNumber,
                            LogisticsId = logistics.LogisticsId,
                            LogisticsName = logistics.Name,
                            CreatedOnUtc = DateTime.UtcNow,
                        }, order);

                        await _orderProcessingManager.ShipAsync(shipment, false);
                    }
                    catch (UserFriendlyException exception)
                    {
                        shipmentDto.Exception = exception.Message;
                        invalidShipments.Add(shipmentDto);
                    }
                    catch (Exception exception)
                    {
                        shipmentDto.Exception = exception.ToString();
                        invalidShipments.Add(shipmentDto);
                    }
                }
                else
                {
                    invalidShipments.Add(shipmentDto);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportShipmentsResultAsync(args, invalidShipments));
        }


        private async Task ProcessImportShipmentsResultAsync(ImportShipmentsFromExcelJobArgs args, List<ImportShipmentDto> invalidShipments)
        {
            if (invalidShipments.Any())
            {
                var file = _invalidUserExporter.ExportToFile(invalidShipments);
                await _appNotifier.SomeShipmentsCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                     L("JobProcessSuccessed"),
                    _localizationSource.GetString("AllShipmentsSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private List<ImportShipmentDto> GetShipmentListFromExcelOrNull(ImportShipmentsFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _shipmentListExcelDataReader.GetShipmentsFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }


        private void SendInvalidExcelNotification(ImportShipmentsFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                L("JobProcessFailed"),
                _localizationSource.GetString("FileCantBeConvertedToShipmentList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }

    }
}
