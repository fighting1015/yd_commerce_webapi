using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.States.Cache
{
    [AutoMapFrom(typeof(Province))]
    public class ProvinceCacheItem
    {
        public const string CacheName = "ProvinceCache";

        public const string CacheByName = "ByProvinceName";


        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }

    [AutoMapFrom(typeof(City))]
    public class CityCacheItem
    {
        public const string CacheName = "CityCache";

        public const string CacheByName = "ByCityName";


        public int Id { get; set; }

        public int ProvinceId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }

    [AutoMapFrom(typeof(District))]
    public class DistrictCacheItem
    {
        public const string CacheName = "DistrictCache";

        public const string CacheByName = "ByDistrictName";

        public int Id { get; set; }

        public int CityId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
