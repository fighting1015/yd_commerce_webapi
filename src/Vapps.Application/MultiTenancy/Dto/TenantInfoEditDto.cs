using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Vapps.MultiTenancy.Dto
{
    [AutoMap(typeof(Tenant))]
    public class TenantInfoEditDto
    {
        /// <summary>
        /// 机构名称/租户名
        /// </summary>
        [Required]
        [StringLength(12)]
        public string TenancyName { get; set; }

        /// <summary>
        /// 宣传语
        /// </summary>
        [StringLength(24)]
        public string Tagline { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Logo 图片 Id
        /// </summary>
        public long LogoId { get; set; }

        /// <summary>
        /// Logo 图片 Url
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 背景图片 Id
        /// </summary>
        public long BackgroundPictureId { get; set; }

        /// <summary>
        /// 背景图片 Url
        /// </summary>
        public string BackgroundPictureUrl { get; set; }
    }
}
