namespace Vapps.Alipay.Infrastructure
{
    public static class ConfigKeys
    {
        public static string ServerAddress => "App:ServerRootAddress";

        public static string IsEnable => "Payment:Alipay:IsEnable";

        public static string NotifyUrl => "Payment:Alipay:NotifyUrl";

        public static string AppId => "Payment:Alipay:AppId";

        public static string Pid => "Payment:Alipay:Pid";

        public static string Gatewayurl => "Payment:Alipay:Gatewayurl";

        public static string PublicKey => "Payment:Alipay:PublicKey";

        public static string PrivateKey => "Payment:Alipay:PrivateKey";

        public static string SignType => "Payment:Alipay:SignType";

        public static string CharSet => "Payment:Alipay:CharSet";

    }
}
