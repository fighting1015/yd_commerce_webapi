using System.Collections.Generic;

namespace Vapps.ECommerce.Catalog.Dto
{
    public class GetCategoryForEditOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
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
        /// 图片Url
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
