using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Consts;

namespace Vapps.States.Dto
{
    [AutoMapFrom(typeof(District))]
    public class CreateOrUpdateDistrictInput : NullableIdDto
    {
        /// <summary>
        /// 城市id
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }

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
