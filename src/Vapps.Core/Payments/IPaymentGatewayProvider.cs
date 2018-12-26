using Abp.Dependency;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Editions;

namespace Vapps.Payments
{
    public interface IPaymentGatewayProvider: ITransientDependency
    {
        string Name { get; }

        bool IsEnable { get; }

        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);

        Task<CreatePaymentResponse> CreateJsPaymentAsync(CreatePaymentRequest request);

        Task<QueryPaymentResponse> QueryPaymentAsync(string paymentId);

        Task<Dictionary<string, string>> GetAdditionalPaymentData(SubscribableEdition edition);
    }
}
