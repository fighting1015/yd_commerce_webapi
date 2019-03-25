using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings
{
    public class ShipmentManager : VappsDomainServiceBase, IShipmentManager
    {
        #region Ctor

        public IRepository<Shipment, long> ShipmentRepository { get; }

        public IQueryable<Shipment> Shipments => ShipmentRepository.GetAll().AsNoTracking();

        public IRepository<ShipmentItem, long> ShipmentItemRepository { get; }

        public IQueryable<ShipmentItem> ShipmentItems => ShipmentItemRepository.GetAll().AsNoTracking();

        public ShipmentManager(IRepository<Shipment, long> ShipmentRepository,
            IRepository<ShipmentItem, long> ShipmentItemRepository)
        {
            this.ShipmentRepository = ShipmentRepository;
            this.ShipmentItemRepository = ShipmentItemRepository;
        }

        #endregion

        #region Shipment

        /// <summary>
        /// 根据订单id查找物流
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public virtual IList<Shipment> FindByOrderId(long orderId, bool readOnly = false)
        {
            if (!readOnly)
                return ShipmentRepository.GetAllIncluding(s => s.Items).Where(s => s.OrderId == orderId).ToList();
            else
                return Shipments.Include(s => s.Items).Where(s => s.OrderId == orderId).ToList();
        }

        /// <summary>
        /// 根据id查找物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Shipment> FindByIdAsync(long id)
        {
            return await ShipmentRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据物流单号查找物流
        /// </summary>
        /// <param name="logisticsNumber"></param>
        /// <returns></returns>
        public virtual async Task<Shipment> FindByLogisticsNumberAsync(string logisticsNumber)
        {
            return await ShipmentRepository.FirstOrDefaultAsync(s => s.LogisticsNumber == logisticsNumber);
        }

        /// <summary>
        /// 根据id获取物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Shipment> GetByIdAsync(long id)
        {
            return await ShipmentRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task CreateAsync(Shipment logistics)
        {
            await ShipmentRepository.InsertAsync(logistics);
        }

        /// <summary>
        /// 更新物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task UpdateAsync(Shipment logistics)
        {
            await ShipmentRepository.UpdateAsync(logistics);
        }

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task DeleteAsync(Shipment logistics)
        {
            await ShipmentRepository.DeleteAsync(logistics);
        }

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(int id)
        {
            var logistics = await ShipmentRepository.FirstOrDefaultAsync(id);

            if (logistics != null)
                await ShipmentRepository.DeleteAsync(logistics);
        }

        #endregion

        #region ShipmentItem

        /// <summary>
        /// 根据id查找租户物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ShipmentItem> FindShipmentItemByIdAsync(int id)
        {
            return await ShipmentItemRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取租户物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ShipmentItem> GetShipmentItemByIdAsync(int id)
        {
            return await ShipmentItemRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加租户物流
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task CreateShipmentItemAsync(ShipmentItem item)
        {
            await ShipmentItemRepository.InsertAsync(item);
        }

        /// <summary>
        /// 更新租户物流
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task UpdateShipmentItemAsync(ShipmentItem item)
        {
            await ShipmentItemRepository.UpdateAsync(item);
        }

        /// <summary>
        /// 删除租户物流
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task DeleteShipmentItemAsync(ShipmentItem item)
        {
            await ShipmentItemRepository.DeleteAsync(item);
        }

        /// <summary>
        /// 删除租户物流
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteShipmentItemAsync(int id)
        {
            var logistics = await ShipmentItemRepository.FirstOrDefaultAsync(id);

            if (logistics != null)
                await ShipmentItemRepository.DeleteAsync(logistics);
        }


        /// <summary>
        /// 删除租户物流
        /// </summary>
        /// <param name="shipmentId"></param>
        public virtual async Task DeleteShipmentItemByShipmentIdAsync(int shipmentId)
        {
            var logistics = await ShipmentItemRepository.FirstOrDefaultAsync(tl => tl.ShipmentId == shipmentId);

            if (logistics != null)
                await ShipmentItemRepository.DeleteAsync(logistics);
        }

        #endregion
    }
}
