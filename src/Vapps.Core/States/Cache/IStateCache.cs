namespace Vapps.States.Cache
{
    public interface IStateCache
    {
        ProvinceCacheItem GetProvince(int id);

        ProvinceCacheItem GetProvinceOrNull(int id);

        ProvinceCacheItem GetProvinceByNameOrNull(string name);

        CityCacheItem GetCity(int id);

        CityCacheItem GetCityOrNull(int id);

        CityCacheItem GetCityByNameOrNull(string name);

        DistrictCacheItem GetDistrict(int id);

        DistrictCacheItem GetDistrictOrNull(int id);

        DistrictCacheItem GetDistrictByNameOrNull(string name);
    }
}
