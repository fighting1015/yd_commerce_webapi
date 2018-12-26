using System;

namespace Vapps.Web.Authentication.External
{
    public class ExternalLoginProviderInfo
    {
        public string Name { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public bool ShowOnLoginPage { get; set; }

        public Type ProviderApiType { get; set; }

        public ExternalLoginProviderInfo(string name, string clientId, string clientSecret, bool showOnLoginPage, Type providerApiType)
        {
            Name = name;
            ClientId = clientId;
            ClientSecret = clientSecret;
            ProviderApiType = providerApiType;
            ShowOnLoginPage = showOnLoginPage;
        }
    }
}