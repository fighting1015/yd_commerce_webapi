using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Files.Dto
{
    /// <summary>
    /// 上传图片回调参数
    /// </summary>
    public class UploadPictureInput
    {
        /// <summary>
        /// 空间名
        /// </summary>
        [Required]
        public string Bucket { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片 Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 创建者id (回调用,必须大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long CreatorUserId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 分组Id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// 图片处理接口
        /// </summary>
        public string ImageMogr2 { get; set; }
    }
}
