using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Threading;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Authorization.Accounts.Cache;
using Vapps.MultiTenancy;
using Vapps.Payments;
using Vapps.Payments.Cache;
using Vapps.WeChat.Core.Users;

namespace Vapps.WeChat.Payments.Jobs
{
    /// <summary>
    /// 处理支付结果
    /// </summary>
    public class ProcessWeChatPaymentJob : BackgroundJob<ProcessWeChatPaymentJobArgs>, ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IAccountCache _accountCache;
        private readonly IPaymentIdCache _paymentIdCache;
        private readonly ISubscriptionPaymentCache _subscriptionPaymentCache;
        private readonly ISubscriptionPaymentManager _subscriptionPaymentManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WeChatUserManager _wechatUserManager;
        private readonly TenantManager _tenantManager;
        private readonly WeChatConfiguration _configuration;

        public ProcessWeChatPaymentJob(ILogger logger,
            IAccountCache accountCache,
            IPaymentIdCache paymentIdCache,
            ISubscriptionPaymentCache subscriptionPaymentCache,
            ISubscriptionPaymentManager subscriptionPaymentManager,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContext,
            IUnitOfWorkManager unitOfWorkManager,
            WeChatUserManager wechatUserManager,
            TenantManager tenantManager,
            WeChatConfiguration configuration)
        {
            this._logger = logger;
            this._accountCache = accountCache;
            this._paymentIdCache = paymentIdCache;
            this._subscriptionPaymentCache = subscriptionPaymentCache;
            this._subscriptionPaymentManager = subscriptionPaymentManager;
            this._objectMapper = objectMapper;
            this._httpContext = httpContext;
            this._tenantManager = tenantManager;
            this._configuration = configuration;
            this._wechatUserManager = wechatUserManager;
            this._unitOfWorkManager = unitOfWorkManager;
            this.LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        [UnitOfWork]
        public override void Execute(ProcessWeChatPaymentJobArgs result)
        {
            AsyncHelper.RunSync(async () =>
            {
                //------------------------------
                //处理业务开始
                //------------------------------
                //处理数据库逻辑
                //注意交易单不要重复处理
                //注意判断返回金额
                var paymentId = result.out_trade_no;

                var subscriptionPaymentCache = await _subscriptionPaymentCache.GetCacheItemOrNullAsync(paymentId);
                if (subscriptionPaymentCache == null)
                {
                    _logger.Error(L("Payments.WeChat.PayFail.PaymentIdNotFound", paymentId));
                    return;
                }

                if (Convert.ToInt32(subscriptionPaymentCache.Amount * 100).ToString() != result.total_fee)
                {
                    _logger.Error(L("Payments.WeChat.PayFail.PaymentAmountNotMatch", paymentId, result.total_fee));
                }
                else
                {
                    string payer = result.openid;
                    var weChatUser = await _wechatUserManager.FindByOnenIdAsync(result.openid);
                    if (weChatUser != null)
                    {
                        var account = await _accountCache.GetAsync(weChatUser.UserId);
                        payer = $"{account.NickName}({result.openid})";
                    }

                    var subscriptionPayment = await _subscriptionPaymentManager.FindByPaymentIdAsync(paymentId);
                    if (subscriptionPayment == null)
                        subscriptionPayment = _objectMapper.Map<SubscriptionPayment>(subscriptionPaymentCache);

                    subscriptionPayment.Gateway = SubscriptionPaymentGatewayType.WeChat;
                    subscriptionPayment.Status = SubscriptionPaymentStatus.Completed;
                    subscriptionPayment.Payer = payer;

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
