using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Media
{
    /// <summary>
    /// 图片资源
    /// </summary>
    [Table("Pictures")]
    public partial class Picture : Entity<long>, IMayHaveTenant, ICreationAudited, IHasCreationTime
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string OriginalUrl { get; set; }

        /// <summary>
        /// 文件 Key(eg:七牛)
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 图片类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 图片分组Id（非外键）
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        public Picture()
        {

        }

        public Picture(int? tenantId, string fileName, string originalUrl, string key = null)
        {
            TenantId = tenantId;
            Name = fileName;
            OriginalUrl = originalUrl;
            Key = key;
        }
    }
}
