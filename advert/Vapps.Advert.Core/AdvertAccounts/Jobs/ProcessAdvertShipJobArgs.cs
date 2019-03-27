namespace Vapps.Advert.AdvertAccounts.Jobs
{
    public class ProcessAdvertShipJobArgs
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 发货记录Id
        /// </summary>
        public long ShipmentId { get; set; }
    }
}
