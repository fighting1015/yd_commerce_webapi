using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Media
{
    [Table("PictureGroups")]
    public class PictureGroup : Entity<long>, IMayHaveTenant, ICreationAudited, IHasCreationTime
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 是否系统分组
        /// </summary>
        public bool IsSystemGroup { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
