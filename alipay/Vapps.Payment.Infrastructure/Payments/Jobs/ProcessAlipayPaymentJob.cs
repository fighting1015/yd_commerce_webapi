using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Threading;
using Alipay.AopSdk.Core.Response;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.MultiTenancy;
using Vapps.Payments;
using Vapps.Payments.Cache;

namespace Vapps.Alipay.Infrastructure.Payments.Jobs
{
    /// 处理支付结果
    /// </summary>
    public class ProcessAlipayPaymentJob : BackgroundJob<AlipayTradePayResponse>, ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IPaymentIdCache _paymentIdCache;
        private readonly ISubscriptionPaymentCache _subscriptionPaymentCache;
        private readonly ISubscriptionPaymentManager _subscriptionPaymentManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly TenantManager _tenantManager;
        private readonly AlipayConfiguration _configuration;

        public ProcessAlipayPaymentJob(ILogger logger,
            IPaymentIdCache paymentIdCache,
            ISubscriptionPaymentCache subscriptionPaymentCache,
            ISubscriptionPaymentManager subscriptionPaymentManager,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContext,
            IUnitOfWorkManager unitOfWorkManager,
            TenantManager tenantManager,
            AlipayConfiguration configuration)
        {
            this._logger = logger;
            this._paymentIdCache = paymentIdCache;
            this._subscriptionPaymentCache = subscriptionPaymentCache;
            this._subscriptionPaymentManager = subscriptionPaymentManager;
            this._objectMapper = objectMapper;
            this._httpContext = httpContext;
            this._tenantManager = tenantManager;
            this._configuration = configuration;
            this._unitOfWorkManager = unitOfWorkManager;
            this.LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        [UnitOfWork]
        public override void Execute(AlipayTradePayResponse result)
        {
            AsyncHelper.RunSync(async () =>
            {
                //------------------------------
                //处理业务开始
                //------------------------------
                //处理数据库逻辑
                //注意交易单不要重复处理
                //注意判断返回金额
                var paymentId = result.OutTradeNo;

                var subscriptionPaymentCache = await _subscriptionPaymentCache.GetCacheItemOrNullAsync(paymentId);
                if (subscriptionPaymentCache == null)
                {
                    _logger.Error(L("Payments.WeChat.PayFail.PaymentIdNotFound", paymentId));
                    return;
                }

                if (subscriptionPaymentCache.Amount.ToString() != result.TotalAmount)
                {
                    _logger.Error(L("Payments.WeChat.PayFail.PaymentAmountNotMatch", paymentId, result.TotalAmount));
                }
                else
                {
                    var subscriptionPayment = await _subscriptionPaymentManager.FindByPaymentIdAsync(paymentId);
                    if (subscriptionPayment == null)
                        subscriptionPayment = _objectMapper.Map<SubscriptionPayment>(subscriptionPaymentCache);

                    subscriptionPayment.Gateway = SubscriptionPaymentGatewayType.Alipay;
                    subscriptionPayment.Status = SubscriptionPaymentStatus.Completed;
                    subscriptionPayment.Payer = result.BuyerLogonId;

                    if (subscriptionPayment.Id > 0)
                        await _subscriptionPaymentManager.UpdateAsync(subscriptionPayment);
                    else
                        await _subscriptionPaymentManager.CreateAsync(subscriptionPayment);

                    using (CurrentUnitOfWork.SetTenantId(subscriptionPayment.TenantId))
                    {
                        await _tenantManager.UpdateTenantAsync(
                            subscriptionPayment.TenantId,
                            true,
                            false,
                            subscriptionPayment.PaymentPeriodType,
                            subscriptionPayment.EditionId,
                            subscriptionPayment.EditionPaymentType
                        );
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }

                // 清除缓存
                _paymentIdCache.RemoveCacheItem(subscriptionPaymentCache.TenantId,
                    subscriptionPaymentCache.EditionId,
                    subscriptionPaymentCache.EditionPaymentType,
                    subscriptionPaymentCache.PaymentPeriodType);

                //_subscriptionPaymentCache.RemoveCacheItem(subscriptionPaymentCache.PaymentId);
            });
        }
    }
}
