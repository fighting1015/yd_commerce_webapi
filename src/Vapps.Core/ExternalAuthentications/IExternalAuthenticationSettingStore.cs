using System.Threading.Tasks;

namespace Vapps.ExternalAuthentications
{
    public interface IExternalAuthenticationSettingStore
    {
        Task<ExternalAuthenticationSetting> GetSettingsAsync(bool getProvider = false);
    }
}
