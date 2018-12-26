using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.States
{
    /// <summary>
    /// 省市区领域服务
    /// </summary>
    public class StateManager : VappsDomainServiceBase, IStateManager
    {
        public IRepository<Province> ProvinceRepository { get; }
        public IRepository<City> CityRepository { get; }
        public IRepository<District> DistrictRepository { get; }

        public IQueryable<Province> Provinces => ProvinceRepository.GetAll().AsNoTracking();
        public IQueryable<City> Cities => CityRepository.GetAll().AsNoTracking();
        public IQueryable<District> Districts => DistrictRepository.GetAll().AsNoTracking();

        public StateManager(IRepository<Province> provinceRepository,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository)
        {
            this.ProvinceRepository = provinceRepository;
            this.CityRepository = cityRepository;
            this.DistrictRepository = districtRepository;
        }

        #region Provinces

        /// <summary>
        /// 根据Id查找省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Province> FindProvinceByIdAsync(int id)
        {
            return await ProvinceRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据Id查找省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Province> FindProvinceByNameAsync(string name)
        {
            return await ProvinceRepository.FirstOrDefaultAsync(p => p.Name.Contains(name));
        }

        /// <summary>
        /// 根据Id获取省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Province> GetProvinceByIdAsync(int id)
        {
            return await ProvinceRepository.GetAsync(id);
        }

        /// <summary>
        /// 创建省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public virtual async Task CreateProvinceAsync(Province province)
        {
            await ProvinceRepository.InsertAsync(province);
        }

        /// <summary>
        /// 更新省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public virtual async Task UpdateProvinceAsync(Province province)
        {
            await ProvinceRepository.UpdateAsync(province);
        }

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public virtual async Task DeleteProvinceAsync(Province province)
        {
            await ProvinceRepository.DeleteAsync(province);
        }

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public virtual async Task DeleteProvinceAsync(int provinceId)
        {
            var province = await GetProvinceByIdAsync(provinceId);

            await DeleteProvinceAsync(province);
        }

        #endregion

        #region Citys

        /// <summary>
        /// 根据Id查找城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<City> FindCityByIdAsync(int id)
        {
            return await CityRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据名称查找城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<City> FindCityByNameAsync(string name)
        {
            return await CityRepository.FirstOrDefaultAsync(p => p.Name.Contains(name));
        }

        /// <summary>
        /// 根据Id获取城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<City> GetCityByIdAsync(int id)
        {
            return await CityRepository.GetAsync(id);
        }

        /// <summary>
        /// 创建城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public virtual async Task CreateCityAsync(City city)
        {
            await CityRepository.InsertAsync(city);
        }

        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public virtual async Task UpdateCityAsync(City city)
        {
            await CityRepository.UpdateAsync(city);
        }

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public virtual async Task DeleteCityAsync(City city)
        {
            await CityRepository.DeleteAsync(city);
        }

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public virtual async Task DeleteCityAsync(int cityId)
        {
            var city = await GetCityByIdAsync(cityId);

            await CityRepository.DeleteAsync(city);
        }

        #endregion

        #region Districts

        /// <summary>
        /// 根据Id查找区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<District> FindDistrictByIdAsync(int id)
        {
            return await DistrictRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据名称查找区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<District> FindDistrictByNameAsync(string name)
        {
            return await DistrictRepository.FirstOrDefaultAsync(p => p.Name.Contains(name));
        }

        /// <summary>
        /// 根据Id获取区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<District> GetDistrictByIdAsync(int id)
        {
            return await DistrictRepository.GetAsync(id);
        }

        /// <summary>
        /// 创建区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public virtual async Task CreateDistrictAsync(District district)
        {
            await DistrictRepository.InsertAsync(district);
        }

        /// <summary>
        /// 更新区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public virtual async Task UpdateDistrictAsync(District district)
        {
            await DistrictRepository.UpdateAsync(district);
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public virtual async Task DeleteDistrictAsync(District district)
        {
            await DistrictRepository.DeleteAsync(district);
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public virtual async Task DeleteDistrictAsync(int districtId)
        {
            var district = await GetDistrictByIdAsync(districtId);

            await DistrictRepository.DeleteAsync(district);
        }

        #endregion
    }
}
