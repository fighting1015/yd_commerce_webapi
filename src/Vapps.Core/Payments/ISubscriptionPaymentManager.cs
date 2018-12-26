using System.Linq;
using System.Threading.Tasks;
using Vapps.Editions.Cache;

namespace Vapps.Payments
{
    public interface ISubscriptionPaymentManager
    {
        ISubscriptionPaymentRepository SubscriptionPaymentRepository { get; }

        IQueryable<SubscriptionPayment> SubscriptionPayments { get; }

        Task<SubscriptionPayment> UpdateByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId, int? tenantId, SubscriptionPaymentStatus status);

        Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId);

        /// <summary>
        /// 根据id获取支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SubscriptionPayment> GetByIdAsync(long id);

        /// <summary>
        /// 根据id获取支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SubscriptionPayment> FindByPaymentIdAsync(string paymentId);

        /// <summary>
        /// 添加支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        Task CreateAsync(SubscriptionPayment subscriptionPayment);

        /// <summary>
        /// 更新支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        Task UpdateAsync(SubscriptionPayment subscriptionPayment);

        /// <summary>
        /// 删除支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        Task DeleteAsync(SubscriptionPayment subscriptionPayment);

        decimal GetUpgradePrice(SubscribableEditionCacheItem currentEdition, SubscribableEditionCacheItem targetEdition, int remainingDaysCount);
    }
}
