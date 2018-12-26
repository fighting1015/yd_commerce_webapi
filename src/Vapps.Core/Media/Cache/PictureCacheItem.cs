using Abp.AutoMapper;
using System;

namespace Vapps.Media.Cache
{
    [AutoMapFrom(typeof(Picture))]
    public partial class PictureCacheItem
    {
        public static string CacheName = "PictureCache";

        public long Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string OriginalUrl { get; set; }

        public string Key { get; set; }

        public string MimeType { get; set; }

        public long GroupId { get; set; }

        public long? CreatorUserId { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}
