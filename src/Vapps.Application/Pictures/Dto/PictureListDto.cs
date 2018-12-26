using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using Vapps.Media;

namespace Vapps.Pictures.Dto
{
    [AutoMap(typeof(Picture))]
    public class PictureListDto : EntityDto<long>, IHasCreationTime
    {
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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
