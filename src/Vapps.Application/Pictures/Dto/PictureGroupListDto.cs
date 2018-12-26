using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;

namespace Vapps.Pictures.Dto
{
    public class PictureGroupListDto : EntityDto<long>, IHasCreationTime
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片数量
        /// </summary>
        public int PictureNum { get; set; }

        /// <summary>
        /// 是否系统分组
        /// </summary>
        public bool IsSystemGroup { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
