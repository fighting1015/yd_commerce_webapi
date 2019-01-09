namespace Vapps.WeChat
{
    public class ConfigKeys
    {
        public static string ServerAddress => "App:ServerRootAddress";

        public static string IsEnable => "Payment:WeChat:IsEnable";

        public static string NotifyUrl => "Payment:WeChat:NotifyUrl";

        public static string AppId => "Payment:WeChat:AppId";

        public static string TenPayKey => "Payment:WeChat:TenPayKey";

        public static string MerchantId => "Payment:WeChat:MerchantId";

        public static string Redis => "Abp:RedisCache:ConnectionString";
    }
}
