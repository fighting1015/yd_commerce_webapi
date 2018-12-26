using System.Threading.Tasks;
using Vapps.Sessions.Dto;

namespace Vapps.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
