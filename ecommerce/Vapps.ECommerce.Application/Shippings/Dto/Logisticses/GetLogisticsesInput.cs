using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.ECommerce.Shippings.Dto
{
    public class GetLogisticsesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 快递名称
        /// </summary>
        public string Name { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }
}
