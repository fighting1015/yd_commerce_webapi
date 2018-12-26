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
        Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input);

        Task<CreatePaymentResponse> CreatePayment(CreatePaymentDto input);

        Task CancelPayment(CancelPaymentDto input);

        Task<CreatePaymentResponse> CreateJsPayment(CreatePaymentDto input);

        Task<QueryPaymentOutput> QueryPayment(string paymentId);

        Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);
    }
}
