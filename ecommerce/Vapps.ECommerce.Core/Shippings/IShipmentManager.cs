using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings
{
    public interface IShipmentManager
    {
        IRepository<Shipment, long> ShipmentRepository { get; }
        IQueryable<Shipment> Shipments { get; }

        IRepository<ShipmentItem, long> ShipmentItemRepository { get; }
        IQueryable<ShipmentItem> ShipmentItems { get; }

        #region Shipment

        /// <summary>
        /// 根据订单id查找物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<Shipment> FindByOrderId(long orderId, bool readOnly = false);

        /// <summary>
        /// 根据id查找发货记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Shipment> FindByIdAsync(long id);

        /// <summary>
        /// 根据物流单号查找物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Shipment> FindByLogisticsNumberAsync(string logisticsNumber);

        /// <summary>
        /// 根据id获取发货记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Shipment> GetByIdAsync(long id);

        /// <summary>
        /// 添加发货记录
        /// </summary>
        /// <param name="Shipment"></param>
        Task CreateAsync(Shipment Shipment);

        /// <summary>
        /// 修改发货记录
        /// </summary>
        /// <param name="Shipment"></param>
        Task UpdateAsync(Shipment Shipment);

        /// <summary>
        /// 删除发货记录
        /// </summary>
        /// <param name="Shipment"></param>
        Task DeleteAsync(Shipment Shipment);

        /// <summary>
        /// 删除发货记录
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        #endregion

        #region ShipmentItem

        /// <summary>
        /// 根据id查找发货条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShipmentItem> FindShipmentItemByIdAsync(int id);

        /// <summary>
        /// 根据id获取发货条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShipmentItem> GetShipmentItemByIdAsync(int id);

        /// <summary>
        /// 添加发货条目
        /// </summary>
        /// <param name="ShipmentItem"></param>
        Task CreateShipmentItemAsync(ShipmentItem ShipmentItem);

        /// <summary>
        /// 修改发货条目
        /// </summary>
        /// <param name="ShipmentItem"></param>
        Task UpdateShipmentItemAsync(ShipmentItem ShipmentItem);

        /// <summary>
        /// 删除发货条目
        /// </summary>
        /// <param name="ShipmentItem"></param>
        Task DeleteShipmentItemAsync(ShipmentItem ShipmentItem);

        /// <summary>
        /// 删除发货条目
        /// </summary>
        /// <param name="id"></param>
        Task DeleteShipmentItemAsync(int id);

        #endregion

    }
}
