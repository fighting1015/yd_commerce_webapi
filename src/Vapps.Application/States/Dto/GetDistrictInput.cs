using System.ComponentModel.DataAnnotations;
using Vapps.Dto;

namespace Vapps.States.Dto
{
    public class GetDistrictInput : PagedAndSortedInputDto
    {
        /// <summary>
        /// 城市Id(必须大于0)
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CtyId { get; set; }
    }
}
