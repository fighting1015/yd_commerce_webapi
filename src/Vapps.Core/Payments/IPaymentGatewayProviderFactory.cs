using Abp.Dependency;

namespace Vapps.Payments
{
    public interface IPaymentGatewayProviderFactory
    {
        IPaymentGatewayProvider Create(SubscriptionPaymentGatewayType gateway);
    }
}
