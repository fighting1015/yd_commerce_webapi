using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vapps.Advert.AdvertAccounts.Sync.Tenant;
using Vapps.Advert.AdvertAccounts.Sync.Toutiao;
using Vapps.Advert.AdvertStatistics;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;
using Vapps.Helpers;
using Vapps.Url;

namespace Vapps.Advert.AdvertAccounts.Sync
{
    public class ToutiaoAdvertAccountSyncor : VappsServiceBase, IAdvertAccountSyncor
    {
        public string AuthAddress => "http://ad.toutiao.com/openapi/audit/oauth.html";

        public string ApiAddressPrefix => "https://ad.toutiao.com/open_api/";

        public string AccessTokenInterface => "oauth2/access_token/";
        public string RefreshAccessTokenInterface => "oauth2/refresh_token/";

        public string FundInterface => "2/advertiser/fund/get/";

        public string DailyReportInterface => "2/report/advertiser/get/";

        private readonly IProductManager _productManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly IAdvertDailyStatisticManager _advertDailyStatisticManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ILogger _logger;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ToutiaoAdvertAccountSyncor(IProductManager productManager,
            IWebUrlService webUrlService,
            IAdvertAccountManager advertAccountService,
            IAdvertDailyStatisticManager advertDailyStatisticManager,
            ILogger logger,
            IDateTimeHelper dateTimeHelper)
        {
            this._webUrlService = webUrlService;
            this._productManager = productManager;
            this._advertAccountManager = advertAccountService;
            this._advertDailyStatisticManager = advertDailyStatisticManager;
            this._logger = logger;
            this._dateTimeHelper = dateTimeHelper;
        }

        public string GetAuthUrl(int accountId)
        {
            var scope = "[\"ad_service\",\"dmp_service\",\"report_service\"]";
            return $"{AuthAddress}?app_id={GetAppId()}&redirect_uri={GetCallBackUrl()}&state={accountId}&scope={scope.UrlEncode()}";
        }

        /// <summary>
        /// 获回调URL
        /// </summary>
        /// <returns></returns>
        private string GetCallBackUrl()
        {
            string storeName = _webUrlService.GetBusinessCenterAddress();
            //var storeName = _storeContext.CurrentStore.Url;
            return string.Format("{0}{1}", storeName, "Admin/AdvertAccount/ToutiaoAuthCallBack");
        }

        public async Task<bool> GetAccessTokenAsync(string code)
        {
            string reqURL = AccessTokenAddress();

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("client_id", await GetAppId()));
            paramList.Add(new KeyValuePair<string, string>("client_secret", await GetAppSecret()));
            paramList.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            paramList.Add(new KeyValuePair<string, string>("authorization_code", code));
            paramList.Add(new KeyValuePair<string, string>("redirect_uri", GetCallBackUrl()));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ToutiaoAdvertResponse<ToutiaoAccesstokenResponse>>(jsonResultString);

                if (result.Code != 0 || result.Data == null)
                    return false;

                var account = await _advertAccountManager.FindByThirdpartyIdAsync(result.Data.AdvertiserId.ToString());
                if (account == null)
                    return false;

                account.AccessToken = result.Data.AccessToken;
                account.AccessTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpiresIn);
                account.RefreshToken = result.Data.RefreshToken;
                account.RefreshTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpiresIn);
                await _advertAccountManager.UpdateAsync(account);

                await QueryAccountFundsAsync(account);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public async Task<bool> RefreshAccessTokenAsync(AdvertAccount account)
        {
            if (DateTime.UtcNow > account.RefreshTokenExpiresIn)
                return false;

            string reqURL = RefessAccessTokenAddress();

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("app_id", await GetAppId()));
            paramList.Add(new KeyValuePair<string, string>("secret", await GetAppSecret()));
            paramList.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            paramList.Add(new KeyValuePair<string, string>("refresh_token", account.RefreshToken));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ToutiaoAdvertResponse<ToutiaoAccesstokenResponse>>(jsonResultString);

                if (result.Code != 0 || result.Data == null)
                    return false;

                account.AccessToken = result.Data.AccessToken;
                account.AccessTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpiresIn);
                account.RefreshToken = result.Data.RefreshToken;
                account.RefreshTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpiresIn);

                await _advertAccountManager.UpdateAsync(account);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<bool> SyncOrders(AdvertAccount account, DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UploadOrderTrackingAsync(Shipment shipment)
        {
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 查询账户资金情况
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> QueryAccountFundsAsync(AdvertAccount account)
        {
            if (!account.IsAuth())
                return false;

            if (account.IsAuthExpires())
                await RefreshAccessTokenAsync(account);

            string reqURL = FundAddress();

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("client_id", await GetAppId()));

            var httpClient = new HttpClient();
            AddAccessToken2Request(account, httpClient);
            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ToutiaoAdvertResponse<ToutiaoFundResponse>>(jsonResultString);

                if (result.Code == 0)
                {
                    account.Balance = result.Data.Balance;
                    await _advertAccountManager.UpdateAsync(account);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }

            return true;
        }

        /// <summary>
        /// 查询每日消耗
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<bool> QueryDailyReportsAsync(AdvertAccount account,
            DateTime startDate,
            DateTime endDate)
        {
            if (!account.IsAuth())
                return false;

            if (account.IsAuthExpires())
                await RefreshAccessTokenAsync(account);

            var product = await _productManager.GetByIdAsync(account.ProductId);
            if (product == null)
                return false;

            string reqURL = DailyReportAddress();

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("advertiser_id", account.ThirdpartyId));
            paramList.Add(new KeyValuePair<string, string>("start_date", startDate.DateString()));
            paramList.Add(new KeyValuePair<string, string>("end_date", endDate.DateString()));
            paramList.Add(new KeyValuePair<string, string>("time_granularity", "STAT_TIME_GRANULARITY_DAILY"));
            paramList.Add(new KeyValuePair<string, string>("page_size", "1000"));

            var httpClient = new HttpClient();
            AddAccessToken2Request(account, httpClient);
            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ToutiaoAdvertResponse<ToutiaoDailyReportListResponse>>(jsonResultString);
                var datas = result.Data.List.OrderByDescending(l => l.StatDatetime).ToList();

                foreach (var item in datas)
                {
                    if (item.AdvertiserId.IsNullOrEmpty())
                        continue;

                    if (item.Cost == decimal.Zero)
                        continue;

                    var dataImport = new AdvertStatisticImport()
                    {
                        AdvertAccountId = account.Id,
                        ProductId = account.ProductId,
                        ProductName = product.Name,
                        StatisticOnUtc = item.StatDatetime.GetStartTimeOfDate(),

                        ClickNum = item.Click,
                        DisplayNum = item.Show,
                        ClickPrice = item.Click > 0 ? Math.Round(item.Cost / item.Click, 4) : 0,
                        ThDisplayCost = item.Show > 0 ? Math.Round(item.Cost / item.Show * 1000, 4) : 0,
                        TotalCost = item.Cost,
                    };

                    await _advertDailyStatisticManager.InsertOrUpdateAdvertStatisticAsync(dataImport);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }

            return true;
        }

        private void AddAccessToken2Request(AdvertAccount account, HttpClient client)
        {
            if (account.AccessToken.IsNullOrEmpty())
                return;

            client.DefaultRequestHeaders.Add("Access-Token", account.AccessToken);
        }

        /// <summary>
        /// accesstoken 获取地址
        /// </summary>
        /// <returns></returns>
        private string AccessTokenAddress()
        {
            return ApiAddressPrefix + AccessTokenInterface;
        }

        /// <summary>
        /// accesstoken 刷新地址
        /// </summary>
        /// <returns></returns>
        private string RefessAccessTokenAddress()
        {
            return ApiAddressPrefix + RefreshAccessTokenInterface;
        }

        /// <summary>
        /// 查询资金余额地址
        /// </summary>
        /// <returns></returns>
        private string FundAddress()
        {
            return ApiAddressPrefix + FundInterface;
        }

        /// <summary>
        /// 查询消耗数据
        /// </summary>
        /// <returns></returns>
        private string DailyReportAddress()
        {
            return ApiAddressPrefix + DailyReportInterface;
        }

        private async Task<string> GetAppSecret()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AdvertSettings.ToutiaoAdsAppId);
        }

        private async Task<string> GetAppId()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AdvertSettings.ToutiaoAdsAppSecret);
        }
    }
}
