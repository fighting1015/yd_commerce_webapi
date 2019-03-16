using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Common.Dto;
using Vapps.Dto;
using Vapps.States.Dto;

namespace Vapps.States
{
    public interface IStateService
    {
        #region Provinces

        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<ProvinceListDto>> GetProvinces(PagedAndSortedInputDto input);

        /// <summary>
        /// 获取所有可用省份(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto<int>>> GetProvinceSelectList();

        /// <summary>
        /// 创建或更新省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateProvince(CreateOrUpdateProvinceInput input);

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProvince(EntityDto input);

        #endregion

        #region Citys

        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<CityListDto>> GetCitys(GetCityInput input);

        /// <summary>
        /// 获取所有可用城市(下拉框)
        /// </summary>
        /// <param name="provinceId">省份id</param>
        /// <returns></returns>
        Task<List<SelectListItemDto<int>>> GetCitySelectList(int provinceId);

        /// <summary>
        /// 创建或更新城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateCity(CreateOrUpdateCityInput input);

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteCity(EntityDto input);

        #endregion

        #region Districts

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<DistrictListDto>> GetDistricts(GetDistrictInput input);

        /// <summary>
        /// 获取所有可用区域(下拉框)
        /// </summary>
        /// <param name="cityId">城市id</param>
        /// <returns></returns>
        Task<List<SelectListItemDto<int>>> GetDistrictSelectList(int cityId);

        /// <summary>
        /// 创建或更新区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateDistrict(CreateOrUpdateDistrictInput input);

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteDistrict(EntityDto input);

        #endregion
    }
}
