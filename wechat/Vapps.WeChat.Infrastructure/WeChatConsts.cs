namespace Vapps.WeChat
{
    public static class WeChatConsts

    {
        public static readonly string WEBPAGENAME = "WeChat";


        public static readonly string MPNAME = "WeChatMP";


        public static string JsapiTicket
        {
            get
            {
                return "jsapi";
            }
        }

        public static string CardTicket
        {
            get
            {
                return "wx_card";
            }
        }

        public static string FirstDataName
        {
            get
            {
                return "first";
            }
        }

        public static string RemarkDataName
        {
            get
            {
                return "remark";
            }
        }
    }
}
