using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Vapps.Advert.AdvertAccounts.Sync.Tenant;
using Vapps.Advert.AdvertStatistics;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;
using Vapps.Helpers;
using Vapps.Url;

namespace Vapps.Advert.AdvertAccounts.Sync
{
    public class TenantAdvertAccountSyncor : VappsServiceBase, IAdvertAccountSyncor
    {
        public string ApiAddress => "https://api.e.qq.com/v1.0/";
        public string OrderInterface => "ecommerce_order/get";
        public string OrderUpdateInterface => "ecommerce_order/update";

        public string DailyRepoersInterface => "daily_reports/get";
        public string FundsInterface => "funds/get";

        private readonly IProductManager _productManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly IAdvertDailyStatisticManager _advertDailyStatisticManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ILogger _logger;
        private readonly IOrderImportor _orderImportor;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILogisticsManager _logisticsManager;

        public TenantAdvertAccountSyncor(IProductManager productManager,
            IWebUrlService webUrlService,
            IAdvertAccountManager advertAccountService,
            IAdvertDailyStatisticManager advertDailyStatisticManager,
            ILogger logger,
            IOrderImportor orderImportor,
            IDateTimeHelper dateTimeHelper,
            ILogisticsManager logisticsManager)
        {
            this._webUrlService = webUrlService;
            this._productManager = productManager;
            this._advertAccountManager = advertAccountService;
            this._advertDailyStatisticManager = advertDailyStatisticManager;
            this._orderImportor = orderImportor;
            this._logger = logger;
            this._dateTimeHelper = dateTimeHelper;
            this._logisticsManager = logisticsManager;
        }

        public string GetAuthUrl(int accountId)
        {
            return $"https://developers.e.qq.com/oauth/authorize?client_id={GetAppId()}&redirect_uri={GetCallBackUrl()}&state={accountId}&scope=";
        }

        /// <summary>
        /// 获回调URL
        /// </summary>
        /// <returns></returns>
        private string GetCallBackUrl()
        {
            string storeName = _webUrlService.GetBusinessCenterAddress();
            //var storeName = _storeContext.CurrentStore.Url;
            return string.Format("{0}{1}", storeName, "Admin/AdvertAccount/TenantAuthCallBack");
        }

        public async Task<bool> GetAccessTokenAsync(string code)
        {
            string reqURL = "https://api.e.qq.com/oauth/token";

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
                var result = JsonConvert.DeserializeObject<TenantAdvertRequestResult<AccesstokenResult>>(jsonResultString);

                if (result.Data == null)
                    return false;

                var account = await _advertAccountManager.FindByUserNameAsync(result.Data.AuthorizerInfo.AccountUIn);
                if (account == null)
                    return false;

                account.AccessToken = result.Data.AccessToken;
                account.AccessTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpiresIn);
                account.RefreshToken = result.Data.RefreshToken;
                account.RefreshTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpiresIn);
                await _advertAccountManager.UpdateAsync(account);

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
            string reqURL = "https://api.e.qq.com/oauth/token";

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("client_id", await GetAppId()));
            paramList.Add(new KeyValuePair<string, string>("client_secret", await GetAppSecret()));
            paramList.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            paramList.Add(new KeyValuePair<string, string>("refresh_token", account.RefreshToken));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TenantAdvertRequestResult<AccesstokenResult>>(jsonResultString);

                account.AccessToken = result.Data.AccessToken;
                account.AccessTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.AccessTokenExpiresIn);
                account.RefreshToken = result.Data.RefreshToken;
                account.RefreshTokenExpiresIn = DateTime.UtcNow.AddSeconds(result.Data.RefreshTokenExpiresIn);
                await _advertAccountManager.UpdateAsync(account);

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

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<bool> SyncOrders(AdvertAccount account, DateTime startDate, DateTime endDate)
        {
            bool readEnd = false;
            int pageIndex = 1;
            int pageSize = 100;

            while (!readEnd)
            {
                var response = await QueryOrderAsync(account, startDate, endDate, pageIndex, pageSize);

                if (response.Code > 0)
                {
                    _logger.Error(string.Format("订单同步失败。{0}", response.Message));
                    break;
                }

                foreach (var orderData in response.Data.Orders)
                {
                    var orderDto = new OrderImport()
                    {
                        OrderSource = OrderSource.Tenant,
                        StoreId = account.StoreId,
                        AdvertAccountId = account.Id,
                        OrderNumber = orderData.OrderId,
                        ProductSku = GetSku(orderData.PackageInfo),
                        OrderStatus = OrderStatus.WaitConfirm,
                        ShippingStatus = ShippingStatus.NotYetShipped,
                        OrderTotal = Convert.ToDecimal(orderData.TotalPrice) / 100,

                        PackageNum = orderData.Quantity,
                        PackageName = orderData.PageName,

                        CustomerComment = GetSelectPackageInfo(orderData),

                        ReceiverName = orderData.UserName,
                        Telephone = orderData.UserPhone,
                        FullAddress = orderData.UserAddress,
                        Address = orderData.UserAddress,
                        Province = orderData.UserProvince,
                        City = orderData.UserCity,
                        District = orderData.UserArea,

                        PlaceOnUtc = orderData.OrderTime.LocalTimeConverUtcTime(_dateTimeHelper),
                        CreatedOnUtc = orderData.OrderTime.LocalTimeConverUtcTime(_dateTimeHelper),
                    };

                    try
                    {
                        await _orderImportor.ImportOrderAsync(orderDto);
                    }
                    catch (Exception exc)
                    {
                        _logger.Error(string.Format("订单同步失败。{0}", exc.Message), exc);
                    }
                }

                readEnd = (pageIndex) * response.Data.PageInfo.PageSize > response.Data.PageInfo.TotalNumber;

                pageIndex += 1;
            }

            return true;
        }

        private static string GetSelectPackageInfo(TenantOrderItem orderData)
        {
            var packageInfos = orderData.PackageInfo.Split(new string[] { ";" },
                        StringSplitOptions.RemoveEmptyEntries);

            if (packageInfos.Length == 1)
            {
                packageInfos = orderData.PackageInfo.Split(new string[] { "\n" },
                StringSplitOptions.RemoveEmptyEntries);
            }

            var selectInfo = packageInfos.Length > 1 ? packageInfos[1] : string.Empty;

            selectInfo = selectInfo.Split(':').Length > 1 ? selectInfo.Split(':')[1] : string.Empty;

            selectInfo = selectInfo.IsNullOrEmpty() ? orderData.UserMessage : $"{selectInfo};{orderData.UserMessage}";

            return selectInfo;
        }

        /// <summary>
        /// 回传物流信息
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public async Task<bool> UploadOrderTrackingAsync(Shipment shipment)
        {
            if (!shipment.Order.AdvertAccountId.HasValue)
                return false;

            if (!shipment.LogisticsId.HasValue)
                return false;

            var account = await _advertAccountManager.GetByIdAsync(Convert.ToInt32(shipment.Order.AdvertAccountId.Value));
            if (account == null)
                return false;

            if (account.IsAuthExpires())
                await RefreshAccessTokenAsync(account);

            var postUrl = OrderUpdateAddress() + GetRequestPara(account);
            if (shipment == null)
                return false;

            using (var client = new HttpClient())
            {
                var logistics = await _logisticsManager.GetByIdAsync(shipment.LogisticsId.Value);

                var content = new FormUrlEncodedContent(new Dictionary<string, string>(){
                    { "account_id", account.ThirdpartyId},
                    { "ecommerce_order_id", shipment.OrderNumber},
                    { "delivery_tracking_number", shipment.LogisticsNumber},
                    { "express_company", GetFxShipEnum(logistics.Memo)},
                    { "ecommerce_order_status", "ORDER_DELIVERING"}
                });

                var response = client.PostAsync(postUrl, content).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;
            }

            return true;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<TenantAdvertRequestResult<TenantOrder>> QueryOrderAsync(AdvertAccount account, DateTime startDate, DateTime endDate, int page, int pageSize = 100)
        {
            var dataParam = new DateRangePara()
            {
                StartDate = startDate.DateString(),
                EndDate = endDate.DateString(),
            };


            string reqURL = OrderQueryAddress() + GetRequestPara(account);

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("account_id", account.ThirdpartyId));
            paramList.Add(new KeyValuePair<string, string>("date_range", JsonConvert.SerializeObject(dataParam)));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            paramList.Add(new KeyValuePair<string, string>("page_size", pageSize.ToString()));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = (await response.Content.ReadAsStringAsync()).HtmlDecode();

                var result = JsonConvert.DeserializeObject<TenantAdvertRequestResult<TenantOrder>>(jsonResultString);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                httpClient.Dispose();
            }
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

            var reqURL = FundsAddress() + GetRequestPara(account);

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("account_id", account.ThirdpartyId));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TenantAdvertRequestResult<TenantAccountFunds>>(jsonResultString);

                if (result.Code == 0)
                {
                    account.Balance = result.Data.Items.Sum(x => x.Balance) / 100;
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
            var reqURL = DailyRepoersAddress() + GetRequestPara(account);

            var dataParam = new DateRangePara()
            {
                StartDate = startDate.DateString(),
                EndDate = endDate.DateString(),
            };

            bool readEnd = false;
            int pageIndex = 1;
            int pageSize = 1000;

            var dateRange = JsonConvert.SerializeObject(dataParam).UrlDecode();

            var httpClient = new HttpClient();
            try
            {
                while (!readEnd)
                {

                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("account_id", account.ThirdpartyId));
                    paramList.Add(new KeyValuePair<string, string>("level", "ADGROUP"));
                    paramList.Add(new KeyValuePair<string, string>("date_range", dateRange));
                    paramList.Add(new KeyValuePair<string, string>("group_by", "[\"date\"]"));
                    paramList.Add(new KeyValuePair<string, string>("page", pageIndex.ToString()));
                    paramList.Add(new KeyValuePair<string, string>("pageSize", pageSize.ToString()));

                    var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                    var jsonResultString = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<TenantAdvertRequestResult<TenantDailyAdvert>>(jsonResultString);

                    foreach (var dailyStatistic in result.Data.Items)
                    {
                        //统计日期
                        var dataOnUtc = dailyStatistic.Date.LocalTimeConverUtcTime(_dateTimeHelper);

                        var dto = new AdvertStatisticImport()
                        {
                            AdvertAccountId = account.Id,
                            ProductId = account.ProductId,
                            ProductName = product.Name,
                            StatisticOnUtc = dailyStatistic.Date,

                            ClickNum = dailyStatistic.Click,
                            DisplayNum = dailyStatistic.Impression,
                            ClickPrice = Math.Round((dailyStatistic.Cost / 100) / dailyStatistic.Click, 4),
                            ThDisplayCost = Math.Round((dailyStatistic.Cost / 100) / dailyStatistic.Impression * 1000, 4),
                            TotalCost = dailyStatistic.Cost / 100,
                        };

                        await _advertDailyStatisticManager.InsertOrUpdateAdvertStatisticAsync(dto);
                    }

                    readEnd = (pageIndex) * pageSize > result.Data.PageInfo.TotalNumber;
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

        private async Task<string> GetAppSecret()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AdvertSettings.TenantAdsAppSecret);
        }

        private async Task<string> GetAppId()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AdvertSettings.TenantAdsAppId);
        }

        private string OrderQueryAddress()
        {
            return ApiAddress + OrderInterface;
        }

        private string OrderUpdateAddress()
        {
            return ApiAddress + OrderUpdateInterface;
        }

        private string DailyRepoersAddress()
        {
            return ApiAddress + DailyRepoersInterface;
        }

        private string FundsAddress()
        {
            return ApiAddress + FundsInterface;
        }

        private string GetRequestPara(AdvertAccount account)
        {
            if (account.AccessToken.IsNullOrEmpty())
                return string.Empty;

            var nonStr = CommonHelper.GenerateRandomDigitCode(6);
            var timeStamp = DateTime.UtcNow.ConvertDateTimeStampLong(milliseconds: false);

            return $"?access_token={account.AccessToken}&timestamp={timeStamp}&nonce={nonStr}";
        }

        private string GetSku(string packageInfo)
        {
            var packageInfos = packageInfo.Split(new string[] { ";" },
                        StringSplitOptions.RemoveEmptyEntries);

            if (packageInfos.Length == 1)
            {
                packageInfos = packageInfo.Split(new string[] { "\n" },
                StringSplitOptions.RemoveEmptyEntries);
            }

            var selectInfo = packageInfos.Length > 1 ? packageInfos[packageInfos.Length - 1] : string.Empty;

            selectInfo = selectInfo.Split(':').Length > 1 ? selectInfo.Split(':')[0] : string.Empty;

            return selectInfo;
        }

        private string GetFxShipEnum(string memo)
        {
            switch (memo)
            {
                case "jd":
                    return "JD_EXPRESS";
                case "yunda":
                    return "YUNDA_EXPRESS";
                case "shunfeng":
                    return "SF_EXPRESS";
                case "shentong":
                    return "STO";
                case "yuantong":
                    return "YTO";
                case "zhongtong":
                    return "ZTO";
                case "tiantian":
                    return "TTK_EXPRESS";
                case "EMS":
                    return "EMS";
                default:
                    return "";
            }

        }
    }
}
