using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders
{
    public interface IOrderManager
    {
        IRepository<Order, long> OrderRepository { get; }
        IQueryable<Order> Orderes { get; }

        IRepository<OrderItem, long> OrderItemRepository { get; }
        IQueryable<OrderItem> OrderItemes { get; }

        #region Order

        /// <summary>
        /// 根据 订单号 查找订单
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        Task<Order> FindByOrderNumberAsync(string orderNumber);

        /// <summary>
        /// 根据id查找订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Order> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Order> GetByIdAsync(long id);

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        Task CreateAsync(Order order);

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="order"></param>
        Task UpdateAsync(Order order);

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="order"></param>
        Task DeleteAsync(Order order);

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion

        #region OrderItem

        /// <summary>
        /// 根据订单id查找订单条目
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<List<OrderItem>> FindOrderItemByOrderIdAsync(long orderId);

        /// <summary>
        /// 根据id查找订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderItem> FindOrderItemByIdAsync(long id);

        /// <summary>
        /// 根据id获取订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderItem> GetOrderItemByIdAsync(long id);

        /// <summary>
        /// 添加订单条目
        /// </summary>
        /// <param name="orderItem"></param>
        Task CreateOrderItemAsync(OrderItem orderItem);

        /// <summary>
        /// 修改订单条目
        /// </summary>
        /// <param name="orderItem"></param>
        Task UpdateOrderItemAsync(OrderItem orderItem);

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="orderItem"></param>
        Task DeleteOrderItemAsync(OrderItem orderItem);

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="id"></param>
        Task DeleteOrderItemAsync(long id);

        #endregion

    }
}
