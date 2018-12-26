using Abp.AutoMapper;

namespace Vapps.SMS.Dto
{
    [AutoMapFrom(typeof(SMSProviderInfo))]
    public class SMSProviderInfoDto
    {
        /// <summary>
        /// 名称(显示名称)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
    }
}
