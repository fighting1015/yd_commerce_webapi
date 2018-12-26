using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.SMS.Dto
{
    public class GetSMSTemplatesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 短信模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短信模板编码
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短信模板供应商
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 是否激活(Null代表所有)
        /// </summary>
        public bool? IsActived { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
