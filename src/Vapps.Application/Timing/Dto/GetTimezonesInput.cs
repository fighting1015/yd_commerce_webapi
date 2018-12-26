using Abp.Configuration;

namespace Vapps.Timing.Dto
{
    public class GetTimezonesInput
    {
        /// <summary>
        /// 默认时区有效范围
        /// </summary>
        public SettingScopes DefaultTimezoneScope { get; set; }
    }
}
