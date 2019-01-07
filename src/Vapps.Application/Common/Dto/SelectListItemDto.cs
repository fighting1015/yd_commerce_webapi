using Abp.AutoMapper;

namespace Vapps.Common.Dto
{
    [AutoMapFrom(typeof(SelectListItem))]
    public class SelectListItemDto
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
