using System.Threading.Tasks;
using Vapps.MultiTenancy.Dto;

namespace Vapps.MultiTenancy
{
    public interface ITenantInfoAppService
    {
        /// <summary>
        /// 获取租户基本信息
        /// </summary>
        /// <returns></returns>
        Task<TenantInfoEditDto> GetTenantInfoForEdit();

        /// <summary>
        /// 更新租户资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateTenantInfo(TenantInfoEditDto input);
    }
}
