namespace Vapps.Authorization.Accounts.Dto
{
    public class SwitchToLinkedAccountOutput
    {
        /// <summary>
        /// 切换账户令牌
        /// </summary>
        public string SwitchAccountToken { get; set; }

        /// <summary>
        /// 租户系统名称
        /// </summary>
        public string TenancyName { get; set; }
    }
}