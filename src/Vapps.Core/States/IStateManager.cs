using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.States
{
    public interface IStateManager
    {
        IRepository<Province> ProvinceRepository { get; }
        IRepository<City> CityRepository { get; }
        IRepository<District> DistrictRepository { get; }

        IQueryable<Province> Provinces { get; }
        IQueryable<City> Cities { get; }
        IQueryable<District> Districts { get; }

        #region Provinces

        /// <summary>
        /// 根据Id查找省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Province> FindProvinceByIdAsync(int id);

        /// <summary>
        /// 根据Id查找省份
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Province> FindProvinceByNameAsync(string name);

        /// <summary>
        /// 根据Id获取省份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Province> GetProvinceByIdAsync(int id);

        /// <summary>
        /// 创建省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        Task CreateProvinceAsync(Province province);

        /// <summary>
        /// 更新省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        Task UpdateProvinceAsync(Province province);

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        Task DeleteProvinceAsync(Province province);

        /// <summary>
        /// 删除省份
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        Task DeleteProvinceAsync(int provinceId);


        #endregion

        #region Citys

        /// <summary>
        /// 根据Id查找城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<City> FindCityByIdAsync(int id);

        /// <summary>
        /// 根据名称查找区域
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<City> FindCityByNameAsync(string name);

        /// <summary>
        /// 根据Id获取城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<City> GetCityByIdAsync(int id);

        /// <summary>
        /// 创建城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task CreateCityAsync(City city);

        /// <summary>
        /// 更新城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task UpdateCityAsync(City city);

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        Task DeleteCityAsync(City city);

        /// <summary>
        /// 删除城市
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task DeleteCityAsync(int cityId);

        #endregion

        #region Districts

        /// <summary>
        /// 根据Id查找区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<District> FindDistrictByIdAsync(int id);

        /// <summary>
        /// 根据名称查找区域
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<District> FindDistrictByNameAsync(string name);

        /// <summary>
        /// 根据Id获取区域
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<District> GetDistrictByIdAsync(int id);

        /// <summary>
        /// 创建区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        Task CreateDistrictAsync(District district);

        /// <summary>
        /// 更新区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        Task UpdateDistrictAsync(District district);

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        Task DeleteDistrictAsync(District district);

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        Task DeleteDistrictAsync(int districtId);

        #endregion
    }
}
