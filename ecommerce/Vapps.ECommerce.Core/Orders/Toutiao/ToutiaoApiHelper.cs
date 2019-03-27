using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders.Toutiao.Requests;
using Vapps.ECommerce.Orders.Toutiao.Responses;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Toutiao
{
    public static class ToutiaoApiHelper
    {
        public static string AppKey { get; set; }

        public static string AppSecret { get; set; }

        #region Utilitis


        /// <summary>
        /// Gets MD5 hash
        /// </summary>
        /// <param name="Input">Input</param>
        /// <param name="Input_charset">Input charset</param>
        /// <returns>Result</returns>
        private static string GetMD5(string Input, string Input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(Input_charset).GetBytes(Input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取放心购api调用签名
        /// </summary>
        /// <param name="method"></param>
        /// <param name="paramJsonString"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private static string GetApiSign(string method, string paramJsonString, string timestamp)
        {
            paramJsonString = SortJson(JToken.Parse(paramJsonString), null);

            var sParaTemp = $"app_key{AppKey}method{method}param_json{paramJsonString}timestamp{timestamp}v1";

            sParaTemp = AppSecret + sParaTemp + AppSecret;

            //sParaTemp = $"{AppSecret}+{sParaTemp}+{AppSecret}";

            string _input_charset = "utf-8";

            return GetMD5(sParaTemp, _input_charset);
        }

        /// <summary>
        /// JSON格式化重新排序
        /// </summary>
        /// <param name="jobj">原始JSON JToken.Parse(string json);</param>
        /// <param name="obj">初始值Null</param>
        /// <returns></returns>
        private static string SortJson(JToken jobj, JToken obj)
        {
            if (obj == null)
            {
                obj = new JObject();
            }
            List<JToken> list = jobj.ToList<JToken>();
            if (jobj.Type == JTokenType.Object)//非数组
            {
                List<string> listsort = new List<string>();
                foreach (var item in list)
                {
                    string name = JProperty.Load(item.CreateReader()).Name;
                    listsort.Add(name);
                }
                listsort.Sort();
                List<JToken> listTemp = new List<JToken>();
                foreach (var item in listsort)
                {
                    listTemp.Add(list.Where(p => JProperty.Load(p.CreateReader()).Name == item).FirstOrDefault());
                }
                list = listTemp;
                //list.Sort((p1, p2) => JProperty.Load(p1.CreateReader()).Name.GetAnsi() - JProperty.Load(p2.CreateReader()).Name.GetAnsi());

                foreach (var item in list)
                {
                    JProperty jp = JProperty.Load(item.CreateReader());
                    if (item.First.Type == JTokenType.Object)
                    {
                        JObject sub = new JObject();
                        (obj as JObject).Add(jp.Name, sub);
                        SortJson(item.First, sub);
                    }
                    else if (item.First.Type == JTokenType.Array)
                    {
                        JArray arr = new JArray();
                        if (obj.Type == JTokenType.Object)
                        {
                            (obj as JObject).Add(jp.Name, arr);
                        }
                        else if (obj.Type == JTokenType.Array)
                        {
                            (obj as JArray).Add(arr);
                        }
                        SortJson(item.First, arr);
                    }
                    else if (item.First.Type != JTokenType.Object && item.First.Type != JTokenType.Array)
                    {
                        (obj as JObject).Add(jp.Name, item.First);
                    }
                }
            }
            else if (jobj.Type == JTokenType.Array)//数组
            {
                foreach (var item in list)
                {
                    List<JToken> listToken = item.ToList<JToken>();
                    List<string> listsort = new List<string>();
                    foreach (var im in listToken)
                    {
                        string name = JProperty.Load(im.CreateReader()).Name;
                        listsort.Add(name);
                    }
                    listsort.Sort();
                    List<JToken> listTemp = new List<JToken>();
                    foreach (var im2 in listsort)
                    {
                        listTemp.Add(listToken.Where(p => JProperty.Load(p.CreateReader()).Name == im2).FirstOrDefault());
                    }
                    list = listTemp;

                    listToken = list;
                    // listToken.Sort((p1, p2) => JProperty.Load(p1.CreateReader()).Name.GetAnsi() - JProperty.Load(p2.CreateReader()).Name.GetAnsi());
                    JObject item_obj = new JObject();
                    foreach (var token in listToken)
                    {
                        JProperty jp = JProperty.Load(token.CreateReader());
                        if (token.First.Type == JTokenType.Object)
                        {
                            JObject sub = new JObject();
                            (obj as JObject).Add(jp.Name, sub);
                            SortJson(token.First, sub);
                        }
                        else if (token.First.Type == JTokenType.Array)
                        {
                            JArray arr = new JArray();
                            if (obj.Type == JTokenType.Object)
                            {
                                (obj as JObject).Add(jp.Name, arr);
                            }
                            else if (obj.Type == JTokenType.Array)
                            {
                                (obj as JArray).Add(arr);
                            }
                            SortJson(token.First, arr);
                        }
                        else if (item.First.Type != JTokenType.Object && item.First.Type != JTokenType.Array)
                        {
                            if (obj.Type == JTokenType.Object)
                            {
                                (obj as JObject).Add(jp.Name, token.First);
                            }
                            else if (obj.Type == JTokenType.Array)
                            {
                                item_obj.Add(jp.Name, token.First);
                            }
                        }
                    }
                    if (obj.Type == JTokenType.Array)
                    {
                        (obj as JArray).Add(item_obj);
                    }

                }
            }
            string ret = obj.ToString(Formatting.None);
            return ret;
        }


        #endregion

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static OrderStatus GetOrderStatus(this ToutiaoOrderStatus status)
        {
            switch (status)
            {
                case ToutiaoOrderStatus.WaitForComfirm:
                    return OrderStatus.WaitConfirm;
                case ToutiaoOrderStatus.WaitForShip:
                    return OrderStatus.Processing;
                case ToutiaoOrderStatus.Shipped:
                    return OrderStatus.Processing;
                case ToutiaoOrderStatus.Canceled:
                    return OrderStatus.Canceled;
                case ToutiaoOrderStatus.Complated:
                    return OrderStatus.Completed;
                case ToutiaoOrderStatus.Refunse:
                    return OrderStatus.Canceled;
                default:
                    return OrderStatus.WaitConfirm;
            }
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static ShippingStatus GetShippingStatus(this ToutiaoOrderStatus status)
        {
            if (status == ToutiaoOrderStatus.Shipped)
                return ShippingStatus.Taked;

            return ShippingStatus.NotYetShipped;
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static PaymentStatus GetPaymentStatus(this ToutiaoOrderStatus status)
        {
            if (status == ToutiaoOrderStatus.Complated)
                return PaymentStatus.Paid;

            return PaymentStatus.Pending;
        }

        /// <summary>
        /// 获取渠道佣金
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="orderTotal"></param>
        /// <returns></returns>
        public static decimal GetOrderReward(this OrderChildItem orderItem, decimal orderTotal)
        {
            switch (orderItem.OrderSource)
            {
                case ToutiaoOrderSource.Charge_Other:
                case ToutiaoOrderSource.Charge_Fxg:
                    return orderTotal * 10 / 100;
                case ToutiaoOrderSource.Charge_Media:
                    return orderTotal * orderItem.CosRatio / 100;
                default:
                    return decimal.Zero;
            }
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public static async Task<ToutiaoDataResponse<OrderListResponse>> GetOrderList(OrderListRequestPara para)
        {
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var paraString = JsonConvert.SerializeObject(para, jSetting);

            var timestamp = DateTime.Now.DateTimeString();
            var sign = GetApiSign(ApiName.OrderList, paraString, timestamp);
            string requestUrl = $"{ApiAddress.OrderList}?app_key={AppKey}&method={ApiName.OrderList}&param_json={paraString}&timestamp={timestamp}&v=1&sign={sign}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(requestUrl));
                var returnResult = await response.Content.ReadAsStringAsync();
                try
                {
                    var deserializeResult = JsonConvert.DeserializeObject<ToutiaoDataResponse<OrderListResponse>>(returnResult);
                    return deserializeResult;
                }
                catch (Exception)
                {
                    return new ToutiaoDataResponse<OrderListResponse>();
                }
            }
        }

        /// <summary>
        /// 回传物流单号
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public static async Task AddLogisticsAsync(Shipment shipment)
        {
            var shipInfo = ToutiaoShipInfoData.GetToutiaoShipInfoByName(shipment.LogisticsName);
            var para = new OrderLogisticsAddRequestPara()
            {
                OrderId = shipment.OrderNumber + "A",
                LogisticsId = shipInfo.Id.ToString(),
                LogisticsCode = shipment.LogisticsNumber,
            };

            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var paraString = JsonConvert.SerializeObject(para, jSetting);

            var timestamp = DateTime.Now.DateTimeString();
            var sign = GetApiSign(ApiName.LogisticsAdd, paraString, timestamp);
            string requestUrl = $"{ApiAddress.LogisticsAdd}?app_key={AppKey}&method={ApiName.LogisticsAdd}&param_json={paraString}&timestamp={timestamp}&v=1&sign={sign}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(requestUrl));
                var returnResult = await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 订单确认
        /// </summary>
        /// <param name="shipment"></param>
        public static async Task OrderStockUpAsync(Shipment shipment)
        {
            var para = new OrderLogisticsAddRequestPara()
            {
                OrderId = shipment.OrderNumber + "A",
            };

            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var paraString = JsonConvert.SerializeObject(para, jSetting);

            var timestamp = DateTime.Now.DateTimeString();
            var sign = GetApiSign(ApiName.LogisticsAdd, paraString, timestamp);
            string requestUrl = $"{ApiAddress.StockUp}?app_key={AppKey}&method={ApiName.StockUp}&param_json={paraString}&timestamp={timestamp}&v=1&sign={sign}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(requestUrl));
                var returnResult = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
