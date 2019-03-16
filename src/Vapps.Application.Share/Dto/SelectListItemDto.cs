using Abp.AutoMapper;

namespace Vapps.Dto
{
    //[AutoMapFrom(typeof(SelectListItem<T>))]
    public class SelectListItemDto<T>
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
    }
}
