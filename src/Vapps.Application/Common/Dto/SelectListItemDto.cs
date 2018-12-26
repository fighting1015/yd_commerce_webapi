using Abp.AutoMapper;

namespace Vapps.Common.Dto
{
    [AutoMapFrom(typeof(SelectListItem))]
    public class SelectListItemDto
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}
