using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.SMS
{
    /// <summary>
    /// 短信供应商信息
    /// </summary>
    public class SMSProviderInfo
    {
        public string Name { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public Type ProviderApiType { get; set; }

        public SMSProviderInfo(string name, string clientId, string clientSecret, Type providerApiType)
        {
            Name = name;
            AppId = clientId;
            AppSecret = clientSecret;
            ProviderApiType = providerApiType;
        }
    }
}
