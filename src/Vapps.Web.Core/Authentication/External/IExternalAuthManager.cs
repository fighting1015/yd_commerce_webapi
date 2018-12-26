using System.Threading.Tasks;
using Vapps.ExternalAuthentications;
using Vapps.Web.Models.TokenAuth;

namespace Vapps.Web.Authentication.External
{
    public interface IExternalAuthManager
    {
        Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode);

        Task<ExternalLoginUserInfo> GetUserInfo(string provider, string accessCode);
    }
}
