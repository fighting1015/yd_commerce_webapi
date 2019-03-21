using Abp.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vapps.Configuration;
using Vapps.ECommerce.Orders;
using Vapps.Helpers;

namespace Vapps.ECommerce.Shippings.Tracking
{
    public class ShipmentTracker : VappsDomainServiceBase, IShipmentTracker
    {
        private readonly IOrderManager _orderManager;
        private readonly IShipmentManager _shipmentManager;
        private readonly ILogisticsManager _logisticsManager;

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IConfigurationRoot _appConfiguration;


        public ShipmentTracker(IOrderManager orderManager,
            IShipmentManager shipmentManager,
            ILogisticsManager logisticsManager,
            IDateTimeHelper dateTimeHelper,
            IAppConfigurationAccessor configurationAccessor)
        {
            this._orderManager = orderManager;
            this._shipmentManager = shipmentManager;
            this._dateTimeHelper = dateTimeHelper;
            this._logisticsManager = logisticsManager;
            this._appConfiguration = configurationAccessor.Configuration;
        }


        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        public async Task UpdateShipmentTraces(List<TraceResult> traces)
        {
            foreach (var tracesItem in traces)
            {
                await UpdateShipmentTracesAsync(tracesItem);
            }
        }

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        public async Task UpdateShipmentTracesAsync(TraceResult trace)
        {
            if (!trace.Traces.Any())
                return;

            var shipment = await _shipmentManager.FindByLogisticsNumberAsync(trace.LogisticCode);
            var logistics = await _logisticsManager.FindByKeyAsync(trace.LogisticCode);

            shipment.ShipmentDetail = JsonConvert.SerializeObject(trace);
            if (!trace.UpdateTime.IsNullOrEmpty())
                shipment.ReceivedOn = Convert.ToDateTime(trace.UpdateTime).LocalTimeConverUtcTime(_dateTimeHelper);

            shipment.Order.ShippingStatus = GetShippingStatus(logistics, shipment.ShipmentDetail, trace.State);

            await _orderManager.UpdateAsync(shipment.Order);
        }

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <returns></returns>
        public async Task<TraceResult> GetShipmentTraces(Shipment shipment, bool refresh)
        {
            var logistics = await _logisticsManager.FindByIdAsync(shipment.LogisticsId.Value);
            TraceResult traces;
            if (refresh || shipment.ShipmentDetail.IsNullOrEmpty())
            {
                traces = await GetOrderTraces(shipment);
                shipment.ShipmentDetail = JsonConvert.SerializeObject(traces);
                if (traces.State == (int)ShippingStatus.Received && !traces.UpdateTime.IsNullOrEmpty())
                {
                    shipment.ReceivedOn = Convert.ToDateTime(traces.UpdateTime).LocalTimeConverUtcTime(_dateTimeHelper);
                }
                else if (shipment.Order.ShippingStatus != ShippingStatus.IssueWithReject
                    && traces.State == (int)ShippingStatus.IssueWithReject
                    && !traces.UpdateTime.IsNullOrEmpty())
                {
                    shipment.ReceivedOn = Convert.ToDateTime(traces.UpdateTime).LocalTimeConverUtcTime(_dateTimeHelper);
                }

                shipment.Order.ShippingStatus = (ShippingStatus)traces.State;
                await _shipmentManager.UpdateAsync(shipment);
            }
            else
            {
                traces = JsonConvert.DeserializeObject<TraceResult>(shipment.ShipmentDetail);
                traces.Traces = traces.Traces.OrderByDescending(t => t.AcceptTime).ToList();


                traces.State = (int)GetShippingStatus(logistics, shipment.ShipmentDetail, traces.State);
            }

            traces.OrderId = shipment.OrderId;

            return traces;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipmentKey"></param>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        public async Task<TraceResult> GetOrderTraces(Shipment shipment)
        {
            var logistics = await _logisticsManager.FindByIdAsync(shipment.LogisticsId.Value);
            TraceResult result = new TraceResult();
            int queryNum = 0;

            var httpClient = new HttpClient();
            do
            {
                try
                {
                    var url = await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiUrl);
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();

                    string requestData = "{'OrderCode':'','ShipperCode':'" + logistics.Key
                 + "','LogisticCode':'" + shipment.LogisticsNumber + "'}";

                    paramList.Add(new KeyValuePair<string, string>("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8)));
                    paramList.Add(new KeyValuePair<string, string>("EBusinessID", await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiId)));
                    paramList.Add(new KeyValuePair<string, string>("RequestType", "1002"));

                    string dataSign = encrypt(requestData, await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiSecret), "UTF-8");

                    paramList.Add(new KeyValuePair<string, string>("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8)));
                    paramList.Add(new KeyValuePair<string, string>("DataType", "2"));

                    var response = await httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList));
                    var jsonResultString = await response.Content.ReadAsStringAsync();

                    if (jsonResultString.Contains("没有可用套餐"))
                        jsonResultString = shipment.ShipmentDetail;

                    result = JsonConvert.DeserializeObject<TraceResult>(jsonResultString);

                    result.Traces = result.Traces.OrderByDescending(t => t.AcceptTime).ToList();
                    result.UpdateTime = result.Traces.FirstOrDefault()?.AcceptTime ?? DateTime.Now.DateTimeString();

                    result.State = (int)GetShippingStatus(logistics, jsonResultString, result.State);
                }
                catch
                {
                    return result;
                }
                finally
                {
                    queryNum++;
                }

            } while (result.State == 0 && queryNum < 3);

            httpClient.Dispose();

            return result;
        }

        /// <summary>
        /// Json方式  物流信息订阅
        /// </summary>
        /// <returns></returns>
        public async Task<bool> OrderTracesSubByJson(Order order)
        {
            var shipment = order.Shipments.FirstOrDefault();
            var logistics = await _logisticsManager.FindByIdAsync(shipment.LogisticsId.Value);

            var requestData = new OrderTracesSubscription()
            {
                OrderCode = order.OrderNumber,
                ShipperCode = logistics.Key,
                LogisticCode = order.Shipments.FirstOrDefault().LogisticsNumber,
                Sender = new SenderOrReceiver()
                {
                    Name = "维普氏科技",
                    Mobile = "18925115769",
                    ProvinceName = "广东省",
                    CityName = "广州市",
                    ExpAreaName = "番禺区",
                    Address = "番禺大道"
                },
                Receiver = new SenderOrReceiver()
                {
                    Name = order.ShippingName,
                    Mobile = order.ShippingPhoneNumber,
                    ProvinceName = order.ShippingProvince ?? string.Empty,
                    CityName = order.ShippingCity ?? string.Empty,
                    ExpAreaName = order.ShippingDistrict ?? string.Empty,
                    Address = order.ShippingAddress ?? string.Empty,
                }
            };

            string reqURL = "http://api.kdniao.com/api/dist";
            var requestJson = JsonConvert.SerializeObject(requestData);
            var dataSign = encrypt(requestJson, await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiSecret), "UTF-8");

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("RequestData", HttpUtility.UrlEncode(requestJson, Encoding.UTF8)));
            paramList.Add(new KeyValuePair<string, string>("EBusinessID", await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiId)));
            paramList.Add(new KeyValuePair<string, string>("RequestType", "1002"));

            paramList.Add(new KeyValuePair<string, string>("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8)));
            paramList.Add(new KeyValuePair<string, string>("DataType", "2"));

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsync(new Uri(reqURL), new FormUrlEncodedContent(paramList));
                var jsonResultString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TraceResult>(jsonResultString);
                return result.Success;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        ///<summary>
        ///电商Sign签名
        ///</summary>
        ///<param name="content">内容</param>
        ///<param name="keyValue">Appkey</param>
        ///<param name="charset">URL编码 </param>
        ///<returns>DataSign签名</returns>
        private string encrypt(String content, String keyValue, String charset)
        {
            if (keyValue != null)
            {
                return base64(MD5(content + keyValue, charset), charset);
            }
            return base64(MD5(content, charset), charset);
        }

        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>
        private string MD5(string str, string charset)
        {
            byte[] buffer = System.Text.Encoding.GetEncoding(charset).GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] somme = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte a in somme)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("X");
                    else
                        ret += a.ToString("X");
                }
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        private string base64(String str, String charset)
        {
            return Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(str));
        }

        /// <summary>
        /// 获回调URL
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        private string GetCallBackUrl()
        {
            string storeUrl = _appConfiguration["App:ServerRootAddress"];
            return string.Format("{0}{1}", storeUrl, "Shippment/Trace");
        }

        /// <summary>
        /// 解析物流状态
        /// </summary>
        /// <param name="shipmentKey"></param>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        private ShippingStatus GetShippingStatus(Logistics logistics, string traceString, int returnStatus)
        {
            var traceInfos = JsonConvert.DeserializeObject<TraceResult>(traceString);

            var shipmentStatus = GetTracesStatus(returnStatus);

            if (traceInfos.Traces == null || !traceInfos.Traces.Any())
                return shipmentStatus;

            if (logistics.Key == "JD")
            {
                if (traceString.Contains("已拒收") || traceString.Contains("换单打印"))
                {
                    shipmentStatus = ShippingStatus.IssueWithReject;

                    return shipmentStatus;
                }
                else if (traceString.Contains("货物已完成配送"))
                {
                    shipmentStatus = ShippingStatus.Received;

                    return shipmentStatus;
                }
                else if (traceString.Contains("再投"))
                {
                    if (shipmentStatus == ShippingStatus.OnPassag)
                        shipmentStatus = ShippingStatus.Issue;

                    return shipmentStatus;
                }
                else if (traceString.Contains("开始配送"))
                {
                    if (shipmentStatus == ShippingStatus.OnPassag)
                        shipmentStatus = ShippingStatus.Delivering;

                    return shipmentStatus;
                }
                else if (traceString.Contains("等待配送"))
                {
                    if (shipmentStatus == ShippingStatus.OnPassag)
                        shipmentStatus = ShippingStatus.DestinationCity;

                    return shipmentStatus;
                }
            }
            else if (logistics.Key == "YD")
            {
                var lastStation = traceInfos.Traces.LastOrDefault().AcceptStation;

                if (traceInfos.Traces.FirstOrDefault(p => p.AcceptStation.Contains("胡红宇")) != null ||
                    (lastStation.Contains("浙江义乌新光公司") && lastStation.Contains("签收")))
                {
                    shipmentStatus = ShippingStatus.IssueWithReject;

                    return shipmentStatus;
                }
            }

            return shipmentStatus;
        }


        /// <summary>
        /// 解析物流状态
        /// </summary>
        /// <param name="shipmentKey"></param>
        /// <param name="trackingNumber"></param>
        /// <returns></returns>
        private ShippingStatus GetTracesStatus(int returnStatus)
        {
            switch ((TraceStatus)returnStatus)
            {
                case TraceStatus.NoTrace:
                    return ShippingStatus.NoTrace;
                case TraceStatus.Taked:
                    return ShippingStatus.Taked;
                case TraceStatus.OnPassag:
                    return ShippingStatus.OnPassag;
                case TraceStatus.DestinationCity:
                    return ShippingStatus.DestinationCity;
                case TraceStatus.Delivering:
                    return ShippingStatus.Delivering;
                case TraceStatus.InBox:
                    return ShippingStatus.Delivering;
                case TraceStatus.Received:
                    return ShippingStatus.Received;
                case TraceStatus.TackedFormBox:
                    return ShippingStatus.Received;
                case TraceStatus.Issue:
                    return ShippingStatus.Issue;
                case TraceStatus.TimeOut2Update:
                    return ShippingStatus.Issue;
                case TraceStatus.RejectByTimeOut:
                    return ShippingStatus.IssueWithReject;
                case TraceStatus.IssueWithReject:
                    return ShippingStatus.IssueWithReject;
                case TraceStatus.TimeOutInBox:
                    return ShippingStatus.Issue;
                default:
                    return ShippingStatus.NoTrace;
            }
        }

    }
}
