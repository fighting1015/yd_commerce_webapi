using Abp.Dependency;
using System;
using System.Linq;

namespace Vapps.Payments
{
    public class PaymentGatewayProviderFactory : IPaymentGatewayProviderFactory, ITransientDependency
    {
        public IPaymentGatewayProvider Create(SubscriptionPaymentGatewayType gateway)
        {
            var providers = IocManager.Instance.ResolveAll<IPaymentGatewayProvider>();
            var provider = providers.FirstOrDefault(p => p.Name == gateway.ToString());

            if (provider == null)
                throw new Exception("Can not create IPaymentGatewayManager for given gateway: " + gateway);

            return provider;
        }
    }
}
