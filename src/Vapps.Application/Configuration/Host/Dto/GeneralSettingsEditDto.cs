using System.ComponentModel.DataAnnotations;

namespace Vapps.Configuration.Host.Dto
{
    public class GeneralSettingsEditDto
    {
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// 这个值只用于比较用户默认时区
        /// </summary>
        public string TimezoneForComparison { get; set; }
    }
}