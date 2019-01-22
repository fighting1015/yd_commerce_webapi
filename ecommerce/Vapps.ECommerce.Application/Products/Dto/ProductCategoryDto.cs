using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    public partial class ProductCategoryDto : EntityDto<long>
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public virtual long CategoryId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual string Name { get; set; }
    }
}
