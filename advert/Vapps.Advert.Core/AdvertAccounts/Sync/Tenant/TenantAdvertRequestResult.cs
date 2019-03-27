using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertAccounts.Sync.Tenant
{
    public class TenantAdvertRequestResult<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class AccesstokenResult
    {
        [JsonProperty("authorizer_info")]
        public AuthorizerInfo AuthorizerInfo { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token_expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

    }

    public class AuthorizerInfo
    {
        [JsonProperty("account_uin")]
        public string AccountUIn { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("scope_list")]
        public string[] ScopeList { get; set; }
    }

    public class PageInfo
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("total_number")]
        public int TotalNumber { get; set; }

        [JsonProperty("total_page")]
        public int TotalPage { get; set; }
    }
}
