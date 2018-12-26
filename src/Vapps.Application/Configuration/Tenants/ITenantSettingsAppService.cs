using System.Threading.Tasks;
using Abp.Application.Services;
using Vapps.Configuration.Tenants.Dto;

namespace Vapps.Configuration.Tenants
{
    /// <summary>
    /// 租户设置
    /// </summary>
    public interface ITenantSettingsAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <returns></returns>
        Task<TenantSettingsEditDto> GetAllSettings();

        /// <summary>
        /// 更新所有设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateAllSettings(TenantSettingsEditDto input);

        /// <summary>
        /// 清除Logo
        /// </summary>
        /// <returns></returns>
        Task ClearLogo();

        /// <summary>
        /// 清除背景图片
        /// </summary>
        /// <returns></returns>
        Task ClearBackgroundPicture();
    }
}
