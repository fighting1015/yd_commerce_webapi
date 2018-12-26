using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Caching;
using Vapps.Common.Dto;
using Vapps.Dto;
using Vapps.States.Dto;

namespace Vapps.States
{
    /// <summary>
    /// 省市区
    /// </summary>
    public class StateService : VappsAppServiceBase, IStateService
    {
        public readonly IStateManager _stateManager;
        public readonly ICacheManager _cacheManager;

        public StateService(IStateManager stateManager,
            ICacheManager cacheManager)
        {
            this._stateManager = stateManager;
            this._cacheManager = cacheManager;
        }

        #region Provinces

        /// <summary>
        /// 获取所有省份
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<ProvinceListDto>> GetProvinces(PagedAndSortedInputDto input)
        {
            var query = _stateManager.Provinces;
            var provinceCount = await query.CountAsync();

            var province = await query
                .OrderBy(t => t.Id)
                .PageBy(input)
                .ToListAsync();

            var provinceListDto = ObjectMapper.Map<List<ProvinceListDto>>(province);

            return new PagedResultDto<ProvinceListDto>(
                provinceCount,
                provinceListDto);
        }

        /// <summary>
        /// 获取所有可用省份(下拉框)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItemDto>> GetProvinceSelectList()
        {
            return await _cacheManager.GetSelectListItemCache().GetAsync(ApplicationCacheNames.AvailableProvince, async () =>
            {
                var query = _stateManager.Provinces.Where(st => st.IsActive);

                var provinces = await query
                    .OrderBy(st => st.DisplayOrder).ThenBy(d => d.Id)
                    .ToListAsync();

                var selectListItemDto = provinces.Select(x =>
                {
                    return new SelectListItemDto
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return selectListItemDto;
            });
        }

        /// <summary>
        /// 创建或更新省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateProvince(CreateOrUpdateProvinceInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateProvince(input);
            }
            else
            {
                await CreateProvince(input);
            }
        }

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteProvince(EntityDto input)
        {
            await _stateManager.DeleteProvinceAsync(input.Id);
        }

        #endregion

        #region Citys

        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<CityListDto>> GetCitys(GetCityInput input)
        {
            var query = _stateManager.Cities
                .WhereIf(input.ProvinceId > 0, c => c.ProvinceId == input.ProvinceId);

            var cityCount = await query.CountAsync();
            var citys = await query
                .OrderBy(t => t.Id)
                .PageBy(input)
                .ToListAsync();

            var cityListDto = ObjectMapper.Map<List<CityListDto>>(citys);
            return new PagedResultDto<CityListDto>(
                cityCount,
                cityListDto);
        }

        /// <summary>
        /// 获取所有可用城市(下拉框)
        /// </summary>
        /// <param name="provinceId">省份id</param>
        /// <returns></returns>
        public async Task<List<SelectListItemDto>> GetCitySelectList(int provinceId)
        {
            string cacheKey = string.Format(ApplicationCacheNames.AvailableCity, provinceId);
            return await _cacheManager.GetSelectListItemCache().GetAsync(cacheKey, async () =>
            {
                var query = _stateManager.Cities.Where(c => c.ProvinceId == provinceId && c.IsActive);

                var cities = await query
                    .OrderBy(st => st.DisplayOrder).ThenBy(d => d.Id)
                    .ToListAsync();

                var selectListItemDto = cities.Select(x =>
                {
                    return new SelectListItemDto
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return selectListItemDto;
            });
        }

        /// <summary>
        /// 创建或更新城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateCity(CreateOrUpdateCityInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateCity(input);
            }
            else
            {
                await CreateCity(input);
            }
        }

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteCity(EntityDto input)
        {
            await _stateManager.DeleteCityAsync(input.Id);
        }

        #endregion

        #region Districts

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<DistrictListDto>> GetDistricts(GetDistrictInput input)
        {
            var query = _stateManager.Districts
                .WhereIf(input.CtyId > 0, c => c.CityId == input.CtyId);

            var districtCount = await query.CountAsync();
            var districts = await query
                .OrderBy(t => t.Id)
                .PageBy(input)
                .ToListAsync();

            var districtListDto = ObjectMapper.Map<List<DistrictListDto>>(districts);
            return new PagedResultDto<DistrictListDto>(
                districtCount,
                districtListDto);
        }

        /// <summary>
        /// 获取所有可用区域(下拉框)
        /// </summary>
        /// <param name="cityId">城市id</param>
        /// <returns></returns>
        public async Task<List<SelectListItemDto>> GetDistrictSelectList(int cityId)
        {
            string cacheKey = string.Format(ApplicationCacheNames.AvailableDistrict, cityId);
            return await _cacheManager.GetSelectListItemCache().GetAsync(cacheKey, async () =>
            {
                var query = _stateManager.Districts.Where(d => d.CityId == cityId && d.IsActive);

                var districts = await query
                    .OrderBy(st => st.DisplayOrder).ThenBy(d => d.Id)
                    .ToListAsync();

                var selectListItemDto = districts.Select(x =>
                {
                    return new SelectListItemDto
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }).ToList();
                return selectListItemDto;
            });
        }

        /// <summary>
        /// 创建或更新区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateDistrict(CreateOrUpdateDistrictInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateDistrict(input);
            }
            else
            {
                await CreateDistrict(input);
            }
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteDistrict(EntityDto input)
        {
            await _stateManager.DeleteDistrictAsync(input.Id);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 创建省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task CreateProvince(CreateOrUpdateProvinceInput input)
        {
            var province = new Province()
            {
                Name = input.Name,
                IsActive = input.IsActive,
                DisplayOrder = input.Display,
            };

            await _stateManager.CreateProvinceAsync(province);
        }

        /// <summary>
        /// 更新省份
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateProvince(CreateOrUpdateProvinceInput input)
        {
            var province = await _stateManager.GetProvinceByIdAsync(input.Id.Value);
            province.Name = input.Name;
            province.IsActive = input.IsActive;
            province.DisplayOrder = input.Display;

            await _stateManager.UpdateProvinceAsync(province);
        }

        /// <summary>
        /// 创建城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task CreateCity(CreateOrUpdateCityInput input)
        {
            var city = new City()
            {
                ProvinceId = input.ProvinceId,
                Name = input.Name,
                IsActive = input.IsActive,
                DisplayOrder = input.Display,
            };

            await _stateManager.CreateCityAsync(city);
        }

        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateCity(CreateOrUpdateCityInput input)
        {
            var city = await _stateManager.GetCityByIdAsync(input.Id.Value);
            city.ProvinceId = input.ProvinceId;
            city.Name = input.Name;
            city.IsActive = input.IsActive;
            city.DisplayOrder = input.Display;

            await _stateManager.UpdateCityAsync(city);
        }

        /// <summary>
        /// 创建区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task CreateDistrict(CreateOrUpdateDistrictInput input)
        {
            var district = new District()
            {
                CityId = input.CityId,
                Name = input.Name,
                IsActive = input.IsActive,
                DisplayOrder = input.Display,
            };

            await _stateManager.CreateDistrictAsync(district);
        }

        /// <summary>
        /// 更新区域
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateDistrict(CreateOrUpdateDistrictInput input)
        {
            var district = await _stateManager.GetDistrictByIdAsync(input.Id.Value);
            district.CityId = input.CityId;
            district.Name = input.Name;
            district.IsActive = input.IsActive;
            district.DisplayOrder = input.Display;

            await _stateManager.UpdateDistrictAsync(district);
        }

        #endregion
    }
}
