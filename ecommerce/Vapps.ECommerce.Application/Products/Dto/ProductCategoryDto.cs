using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    public partial class ProductCategoryDto
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 分类名称(非必填)
        /// </summary>
        public virtual string Name { get; set; }
    }
}
