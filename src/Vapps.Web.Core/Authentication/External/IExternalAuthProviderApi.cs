using System.Threading.Tasks;
using Vapps.ExternalAuthentications;
using Vapps.Web.Models.TokenAuth;

namespace Vapps.Web.Authentication.External
{
    public interface IExternalAuthProviderApi
    {
        ExternalLoginProviderInfo ProviderInfo { get; }

        Task<bool> IsValidUser(string userId, string accessCode);

        Task<ExternalLoginUserInfo> GetUserInfo(string accessCode);

        void Initialize(ExternalLoginProviderInfo providerInfo);
    }
}