using Abp.Dependency;
using System.Collections.Generic;

namespace Vapps.SMS
{
    public class SMSConfiguration : ISMSConfiguration, ISingletonDependency
    {
        public List<SMSProviderInfo> Providers { get; }

        public SMSConfiguration()
        {
            Providers = new List<SMSProviderInfo>();
        }
    }
}
