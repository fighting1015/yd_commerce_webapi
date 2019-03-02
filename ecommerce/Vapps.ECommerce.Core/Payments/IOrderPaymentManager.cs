using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Payments
{
    public interface IOrderPaymentManager
    {
        IRepository<OrderPayment, long> OrderPaymentRepository { get; }

        IQueryable<OrderPayment> OrderPayments { get; }
      
        #region OrderPayment

        /// <summary>
        /// 根据id查找订单支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderPayment> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取订单支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderPayment> GetByIdAsync(long id);

        /// <summary>
        /// 添加订单支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        Task CreateAsync(OrderPayment orderPayment);

        /// <summary>
        /// 修改订单支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        Task UpdateAsync(OrderPayment orderPayment);

        /// <summary>
        /// 删除订单支付记录
        /// </summary>
        /// <param name="orderPayment"></param>
        Task DeleteAsync(OrderPayment orderPayment);

        /// <summary>
        /// 删除订单支付记录
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
