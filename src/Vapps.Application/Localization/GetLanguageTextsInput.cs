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
        /// �������(ҳ��С)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int MaxResultCount { get; set; } //0: Unlimited.

        /// <summary>
        /// ��������(ҳ��С * ҳ��)
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        /// <summary>
        /// �����ֶ�(eg : Key Desc )
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// Դ����(eg:Vapps,AbpZero,Abp) 
        /// </summary>
        [Required]
        [MaxLength(ApplicationLanguageText.MaxSourceNameLength)]
        public string SourceName { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public string BaseLanguageName { get; set; }

        /// <summary>
        /// Ŀ����������
        /// </summary>
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength, MinimumLength = 2)]
        public string TargetLanguageName { get; set; }

        /// <summary>
        /// Ŀ��ֵ����
        /// </summary>
        public string TargetValueFilter { get; set; }

        /// <summary>
        /// �����ı�
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