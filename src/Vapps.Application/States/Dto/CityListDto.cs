using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Vapps.States.Dto
{
    [AutoMapFrom(typeof(City))]
    public class CityListDto : EntityDto
    {
        /// <summary>
        /// 名称
        /// </summary>
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
