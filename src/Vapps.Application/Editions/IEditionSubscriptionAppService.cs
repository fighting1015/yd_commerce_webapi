using Abp.Application.Services;
using System.Threading.Tasks;
using Vapps.Editions.Dto;

namespace Vapps.Editions
{
    public interface IEditionSubscriptionAppService : IApplicationService
    {
        /// <summary>
        /// 获取版本销售信息
        /// </summary>
        /// <returns></returns>
        Task<EditionsSelectOutput> GetEditionsForSelect();

        /// <summary>
        /// 获取当前账号版本信息
        /// </summary>
        /// <returns></returns>
        Task<EditionsViewOutput> GetCurrentEdition();

        /// <summary>
        /// 获取版本详情
        /// </summary>
        /// <param name="editionId"></param>
        /// <returns></returns>
        Task<EditionSelectDto> GetEdition(int editionId);

        /// <summary>
        /// 版本试用
        /// </summary>
        /// <param name="editionId"></param>
        /// <returns></returns>
        Task TrialEdition(int editionId);

        /// <summary>
        /// 升级到等价版本(价格相等)
        /// </summary>
        /// <param name="upgradeEditionId"></param>
        /// <returns></returns>
        Task UpgradeTenantToEquivalentEdition(int upgradeEditionId);
    }
}
