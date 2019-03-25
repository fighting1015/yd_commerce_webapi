using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Vapps.MultiTenancy.Dto;
using Vapps.Payments;
using Vapps.Payments.Dto;
using Vappse.MultiTenancy.Payments.Dto;

namespace Vapps.Payments
{
    public interface IPaymentAppService : IApplicationService
    {
        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input);

        /// <summary>
        /// 创建支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentDto input);

        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CancelPayment(CancelPaymentDto input);

        /// <summary>
        /// 创建Js支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreatePaymentResponse> CreateJsPayment(CreatePaymentDto input);

        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<QueryPaymentOutput> QueryPayment(string paymentId);

        /// <summary>
        /// 获取支付记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);
    }
}
