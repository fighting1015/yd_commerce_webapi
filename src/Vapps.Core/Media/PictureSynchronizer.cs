using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Session;
using System.Linq;

namespace Vapps.Media
{
    public class PictureSynchronizer :
        IEventHandler<EntityDeletedEventData<Picture>>,
        ITransientDependency
    {
        public IRepository<Picture, long> _pictureRepository { get; }

        private readonly IStorageProvider _storageProvider;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PictureSynchronizer(
            IRepository<Picture, long> pictureRepository,
            IStorageProvider storageProvider,
            IAbpSession abpSession,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _storageProvider = storageProvider;
            _abpSession = abpSession;
            _pictureRepository = pictureRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Handles creation event of user
        /// </summary>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Picture> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
            {
                _storageProvider.DeleteAsync(eventData.Entity.Key);
            }
        }
    }
}
