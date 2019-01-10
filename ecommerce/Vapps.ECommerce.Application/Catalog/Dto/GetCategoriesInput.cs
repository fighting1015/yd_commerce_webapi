using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.ECommerce.Catalog.Dto
{
    public class GetCategoriesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父分类Id(0为所有)
        /// </summary>
        public int ParentCategoryId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
