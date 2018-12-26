using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Vapps.Dto
{
    public class PagedInputDto : IPagedResultRequest
    {
        /// <summary>
        /// 最大结果数量(等同:PageSize)
        /// </summary>
        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        /// <summary>
        /// 列表跳过数量(等同: PageSize*PageIndex)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}