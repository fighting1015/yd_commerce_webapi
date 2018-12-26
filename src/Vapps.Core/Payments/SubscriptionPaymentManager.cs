using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Editions.Cache;

namespace Vapps.Payments
{
    public class SubscriptionPaymentManager : VappsDomainServiceBase, ISubscriptionPaymentManager
    {
        public ISubscriptionPaymentRepository SubscriptionPaymentRepository { get; }

        public IQueryable<SubscriptionPayment> SubscriptionPayments => SubscriptionPaymentRepository.GetAll().AsNoTracking();

        public SubscriptionPaymentManager(ISubscriptionPaymentRepository subscriptionPaymentRepository)
        {
            this.SubscriptionPaymentRepository = subscriptionPaymentRepository;
        }

        public async Task<SubscriptionPayment> UpdateByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId, int? tenantId, SubscriptionPaymentStatus status)
        {
            return await SubscriptionPaymentRepository.UpdateByGatewayAndPaymentIdAsync(gateway, paymentId, tenantId, status);
        }

        public async Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return await SubscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(gateway, paymentId);
        }

        /// <summary>
        /// 根据id获取支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SubscriptionPayment> GetByIdAsync(long id)
        {
            return await SubscriptionPaymentRepository.GetAsync(id);
        }

        /// <summary>
        /// 根据id获取支付记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SubscriptionPayment> FindByPaymentIdAsync(string paymentId)
        {
            return await SubscriptionPaymentRepository.FirstOrDefaultAsync(s => s.PaymentId == paymentId);
        }

        /// <summary>
        /// 添加支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        public virtual async Task CreateAsync(SubscriptionPayment subscriptionPayment)
        {
            await SubscriptionPaymentRepository.InsertAsync(subscriptionPayment);
        }

        /// <summary>
        /// 更新支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        public virtual async Task UpdateAsync(SubscriptionPayment subscriptionPayment)
        {
            await SubscriptionPaymentRepository.UpdateAsync(subscriptionPayment);
        }

        /// <summary>
        /// 删除支付记录
        /// </summary>
        /// <param name="subscriptionPayment"></param>
        public virtual async Task DeleteAsync(SubscriptionPayment subscriptionPayment)
        {
            await SubscriptionPaymentRepository.DeleteAsync(subscriptionPayment);
        }

        public decimal GetUpgradePrice(SubscribableEditionCacheItem currentEdition, SubscribableEditionCacheItem targetEdition, int remainingDaysCount)
        {
            decimal additionalPrice;

            // If remainingDaysCount is longer than annual, then calculate price with using
            // both annual and monthly prices
            if (remainingDaysCount > (int)PaymentPeriodType.Annual)
            {
                var remainingsYearsCount = remainingDaysCount / (int)PaymentPeriodType.Annual;
                remainingDaysCount = remainingDaysCount % (int)PaymentPeriodType.Annual;

                additionalPrice = GetMonthlyCalculatedPrice(currentEdition, targetEdition, remainingDaysCount);
                additionalPrice += GetAnnualCalculatedPrice(currentEdition, targetEdition, remainingsYearsCount); // add yearly price to montly calculated price
            }
            else
            {
                additionalPrice = GetMonthlyCalculatedPrice(currentEdition, targetEdition, remainingDaysCount);
            }

            return additionalPrice;
        }

        private static decimal GetMonthlyCalculatedPrice(SubscribableEditionCacheItem currentEdition, SubscribableEditionCacheItem upgradeEdition, int remainingDaysCount)
        {
            decimal currentUnusedPrice = 0;
            decimal upgradeUnusedPrice = 0;

            if (currentEdition.MonthlyPrice.HasValue)
            {
                currentUnusedPrice = (currentEdition.MonthlyPrice.Value / (int)PaymentPeriodType.Monthly) * remainingDaysCount;
            }

            if (upgradeEdition.MonthlyPrice.HasValue)
            {
                upgradeUnusedPrice = (upgradeEdition.MonthlyPrice.Value / (int)PaymentPeriodType.Monthly) * remainingDaysCount;
            }

            var additionalPrice = upgradeUnusedPrice - currentUnusedPrice;
            return decimal.Round(additionalPrice, 2);
        }

        private static decimal GetAnnualCalculatedPrice(SubscribableEditionCacheItem currentEdition, SubscribableEditionCacheItem upgradeEdition, int remainingYearsCount)
        {
            var currentUnusedPrice = (currentEdition.AnnualPrice ?? 0) * remainingYearsCount;
            var upgradeUnusedPrice = (upgradeEdition.AnnualPrice ?? 0) * remainingYearsCount;

            var additionalPrice = upgradeUnusedPrice - currentUnusedPrice;
            return additionalPrice;
        }

    }
}
