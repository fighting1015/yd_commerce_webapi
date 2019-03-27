using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using Vapps.Advert.AdvertAccounts.Jobs;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Events;

namespace Vapps.Advert.AdvertAccounts.Events
{
    /// <summary>
    /// 订单事件订阅者
    /// </summary>
    public class ShipmentSubscriber :
           IEventHandler<ShipmentSentEvent>,
           IEventHandler<ShipmentDeliveredEvent>,
           IEventHandler<ShipmentRejectedEvent>,
           ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IShipmentManager _shipmentManager;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public ShipmentSubscriber(IUnitOfWorkManager unitOfWorkManager,
            IShipmentManager shipmentManager,
            IBackgroundJobManager backgroundJobManager)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._shipmentManager = shipmentManager;
            this._backgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// 发货事件
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void HandleEvent(ShipmentSentEvent eventData)
        {
            _backgroundJobManager.Enqueue<ProcessAdvertShipJob, ProcessAdvertShipJobArgs>(new ProcessAdvertShipJobArgs()
            {
                TenantId = eventData.Shipment.TenantId,
                ShipmentId = eventData.Shipment.Id,
            });
        }

        /// <summary>
        /// 订单确认
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(ShipmentDeliveredEvent eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {

            }
        }

        /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(ShipmentRejectedEvent eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {

            }
        }
    }
}
