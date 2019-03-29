namespace Vapps.Advert.AdvertAccounts.Dto
{
    public class GetAccessTokenInput
    {
        /// <summary>
        /// 账户Id
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string Code { get; set; }
    }
}
