using System;
using System.Threading.Tasks;
using Vapps.ECommerce.Shippings;

namespace Vapps.Advert.AdvertAccounts
{
    public interface IAdvertAccountSyncor
    {
        string GetAuthUrl(int accountId);

        Task<bool> GetAccessTokenAsync(string code);

        Task<bool> RefreshAccessTokenAsync(AdvertAccount account);

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<bool> SyncOrders(AdvertAccount account, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 回传物流信息
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        Task<bool> UploadOrderTrackingAsync(Shipment shipment);

        /// <summary>
        /// 查询账户资金情况
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<bool> QueryAccountFundsAsync(AdvertAccount account);

        /// <summary>
        /// 查询每日消耗
        /// </summary>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<bool> QueryDailyReportsAsync(AdvertAccount account,
            DateTime startDate,
            DateTime endDate);
    }
}
