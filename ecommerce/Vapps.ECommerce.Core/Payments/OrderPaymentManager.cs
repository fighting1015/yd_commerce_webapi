using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Payments
{
    public class OrderPaymentManager : VappsDomainServiceBase, IOrderPaymentManager
    {
        #region Ctor

        public IRepository<OrderPayment, long> OrderPaymentRepository { get; }

        public IQueryable<OrderPayment> OrderPayments => OrderPaymentRepository.GetAll().AsNoTracking();

        public OrderPaymentManager(IRepository<OrderPayment, long> OrderPaymentRepository)
        {
            this.OrderPaymentRepository = OrderPaymentRepository;
        }

        #endregion

        #region OrderPayment

        /// <summary>
        /// 根据id查找支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderPayment> FindByIdAsync(long id)
        {
            return await OrderPaymentRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderPayment> GetByIdAsync(long id)
        {
            return await OrderPaymentRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        public virtual async Task CreateAsync(OrderPayment orderPayment)
        {
            await OrderPaymentRepository.InsertAsync(orderPayment);
        }

        /// <summary>
        /// 更新支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        public virtual async Task UpdateAsync(OrderPayment orderPayment)
        {
            await OrderPaymentRepository.UpdateAsync(orderPayment);
        }

        /// <summary>
        /// 删除支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        public virtual async Task DeleteAsync(OrderPayment orderPayment)
        {
            await OrderPaymentRepository.DeleteAsync(orderPayment);
        }

        /// <summary>
        /// 删除支付记录
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var orderPayment = await OrderPaymentRepository.FirstOrDefaultAsync(id);

            if (orderPayment != null)
                await OrderPaymentRepository.DeleteAsync(orderPayment);
        }

        #endregion
    }
}
