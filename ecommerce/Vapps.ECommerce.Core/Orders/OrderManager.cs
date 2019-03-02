using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders
{
    public class OrderManager : VappsDomainServiceBase, IOrderManager
    {
        #region Ctor

        public IRepository<Order, long> OrderRepository { get; }

        public IQueryable<Order> Orderes => OrderRepository.GetAll().AsNoTracking();

        public IRepository<OrderItem, long> OrderItemRepository { get; }

        public IQueryable<OrderItem> OrderItemes => OrderItemRepository.GetAll().AsNoTracking();

        public OrderManager(IRepository<Order, long> OrderRepository,
            IRepository<OrderItem, long> OrderItemRepository)
        {
            this.OrderRepository = OrderRepository;
            this.OrderItemRepository = OrderItemRepository;
        }

        #endregion

        #region Order

        /// <summary>
        /// 根据id查找订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Order> FindByIdAsync(long id)
        {
            return await OrderRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Order> GetByIdAsync(long id)
        {
            return await OrderRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task CreateAsync(Order order)
        {
            await OrderRepository.InsertAsync(order);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task UpdateAsync(Order order)
        {
            await OrderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task DeleteAsync(Order order)
        {
            await OrderRepository.DeleteAsync(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var order = await OrderRepository.FirstOrDefaultAsync(id);

            if (order != null)
                await OrderRepository.DeleteAsync(order);
        }

        #endregion

        #region OrderItem

        /// <summary>
        /// 根据订单id查找订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<List<OrderItem>> FindOrderItemByOrderIdAsync(long orderId)
        {
            return await OrderItemRepository.GetAll().Where(tl => tl.OrderId == orderId).ToListAsync();
        }

        /// <summary>
        /// 根据id查找订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderItem> FindOrderItemByIdAsync(long id)
        {
            return await OrderItemRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderItem> GetOrderItemByIdAsync(long id)
        {
            return await OrderItemRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task CreateOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.InsertAsync(item);
        }

        /// <summary>
        /// 更新订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task UpdateOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.UpdateAsync(item);
        }

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task DeleteOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.DeleteAsync(item);
        }

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteOrderItemAsync(long id)
        {
            var order = await OrderItemRepository.FirstOrDefaultAsync(id);

            if (order != null)
                await OrderItemRepository.DeleteAsync(order);
        }

        #endregion
    }
}
