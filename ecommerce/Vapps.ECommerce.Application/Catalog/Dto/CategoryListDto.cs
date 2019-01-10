namespace Vapps.ECommerce.Catalog.Dto
{
    //[AutoMapFrom(typeof(Category))]
    public class CategoryListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类名称:面包屑(父分 >> 子分类)
        /// </summary>
        public string Breadcrumb { get; set; }

        /// <summary>
        /// 父分类Id
        /// </summary>
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 图片Url(图片Id为0的话为空)
        /// </summary>
        public string PictureUrl { get; set; }

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
