using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vapps.Consts;

namespace Vapps.States.Dto
{
    [AutoMapFrom(typeof(City))]
    public class CreateOrUpdateCityInput : NullableIdDto
    {
        /// <summary>
        /// 省份id
        /// </summary>
        [Range(1, int.MaxValue)]
        public int ProvinceId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(EntityConsts.MaxEntityNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Display { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
