using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Validation;

namespace Vapps.Localization
{
    public class GetLanguageTextsInput : IPagedResultRequest, ISortedResultRequest, IShouldNormalize
    {
        /// <summary>
        /// 结果数量(页大小)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int MaxResultCount { get; set; } //0: Unlimited.

        /// <summary>
        /// 跳过数量(页大小 * 页码)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        /// <summary>
        /// 排序字段(eg : Key Desc )
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 源名称(eg:Vapps,AbpZero,Abp) 
        /// </summary>
        [Required]
        [MaxLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }

        /// <summary>
        /// 基础语言名称
        /// </summary>
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string BaseLanguageName { get; set; }

        /// <summary>
        /// 目标语言名称
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength, MinimumLength = 2)]
        public string TargetLanguageName { get; set; }

        /// <summary>
        /// 目标值过滤
        /// </summary>
        public string TargetValueFilter { get; set; }

        /// <summary>
        /// 过滤文本
        /// </summary>
        public string FilterText { get; set; }

        public void Normalize()
        {
            if (TargetValueFilter.IsNullOrEmpty())
            {
                TargetValueFilter = "ALL";
            }
        }
    }
}