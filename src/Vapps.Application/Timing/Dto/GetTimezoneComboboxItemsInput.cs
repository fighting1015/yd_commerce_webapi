using Abp.Configuration;

namespace Vapps.Timing.Dto
{
    public class GetTimezoneComboboxItemsInput
    {
        /// <summary>
        /// 默认时区范围
        /// </summary>
        public SettingScopes DefaultTimezoneScope;

        /// <summary>
        /// 选择时区Id
        /// </summary>
        public string SelectedTimezoneId { get; set; }
    }
}
