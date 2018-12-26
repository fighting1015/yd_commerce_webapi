using System.Collections.Generic;

namespace Vapps.SMS
{
    public interface ISMSConfiguration
    {
        List<SMSProviderInfo> Providers { get; }
    }
}
