using System.ComponentModel.DataAnnotations;
using Vapps.Dto;

namespace Vapps.States.Dto
{
    public class GetCityInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 省份Id(必须大于0)
        /// </summary>
        [Range(1, int.MaxValue)]
        public int ProvinceId { get; set; }
    }
}
