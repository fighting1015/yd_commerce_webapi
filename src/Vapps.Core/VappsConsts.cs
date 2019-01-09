namespace Vapps
{
    public class VappsConsts
    {
        public const string LocalizationSourceName = "Vapps";

        public const string UserLocalizationSourceName = "UserCenter";

        public const string AdminLocalizationSourceName = "Admin";

        public const string ServerSideLocalizationSourceName = "ServerSide";

        public const string BusinessLocalizationSourceName = "BusinessCenter";

        public const string ConnectionStringName = "Default";

        public const string SMSProviderLocalizationName = "SmsProvider.{0}";

        public const bool MultiTenancyEnabled = true;

        public const int PaymentCacheDurationInMinutes = 30;

        public const string WeChatLogin = "WeChat";

        public const string WeChatMpLogin = "WeChatMp";

        public const string QQLogin = "QQ";
    }

    public static class ApplicationCacheNames
    {
        public const string SelectItem = "SelectItem";

        //public const string OutletCaches = "OutletCaches";

        //public const string ContactorCaches = "ContactorCaches";

        public const string AvailableSmsTemplate = "AvailableSmsTemplate";

        public const string AvailableProvince = "AvailableProvince";

        /// <summary>
        /// {0} province id
        /// </summary>
        public const string AvailableCity = "AvailableCity-{0}";

        /// <summary>
        /// {0} city id
        /// </summary>
        public const string AvailableDistrict = "AvailableDistrict-{0}";

        /// <summary>
        /// {0} outlet id
        /// </summary>
        public const string AvailableContactor = "AvailableContactor-{0}";

        /// <summary>
        /// {0} ip 
        /// </summary>
        public const string IpAddress = "IpAddress";
    }
}