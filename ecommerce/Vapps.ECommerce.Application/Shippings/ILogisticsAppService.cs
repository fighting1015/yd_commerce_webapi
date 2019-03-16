using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Shippings.Dto;
using Vapps.ECommerce.Shippings.Dto.Logisticses;

namespace Vapps.ECommerce.Shippings
{
    public interface ILogisticsAppService
    {
        #region Logistics

        /// <summary>
        /// 获取所有物流
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<LogisticsListDto>> GetLogisticses(GetLogisticsesInput input);

        /// <summary>
        /// 获取所有可用物流(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto<long>>> GetLogisticsSelectList();

        /// <summary>
        /// 获取物流详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLogisticsForEditOutput> GetLogisticsForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateLogistics(CreateOrUpdateLogisticsInput input);

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteLogistics(BatchInput<int> input);

        #endregion

        #region Tenant Logistics

        /// <summary>
        /// 获取所有物流
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<TenantLogisticsDto>> GetTenantLogisticses(GetLogisticsesInput input);

        /// <summary>
        /// 获取所有可用物流(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto<long>>> GetTenantLogisticsSelectList();

        /// <summary>
        /// 获取物流详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLogisticsForEditOutput> GetTenantLogisticsForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateTenantLogistics(CreateOrUpdateTenantLogisticsInput input);

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteTenantLogistics(BatchInput<int> input);

        #endregion
    }
}
