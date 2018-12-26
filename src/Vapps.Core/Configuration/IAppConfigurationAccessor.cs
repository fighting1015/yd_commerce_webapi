using Microsoft.Extensions.Configuration;

namespace Vapps.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
