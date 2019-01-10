using System.ComponentModel.DataAnnotations;

namespace Vapps.ECommerce.Catalog.Dto
{
    //[AutoMapFrom(typeof(Store))]
    public class CreateOrUpdateCategoryInput
    {
        /// <summary>
        /// Id，空或者为0时创建分类
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(Category.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 父分类Id
        /// </summary>
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序Id
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
