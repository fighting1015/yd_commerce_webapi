namespace Vapps.Authorization.Accounts.Dto
{
    public class ImpersonateOutput
    {
        /// <summary>
        /// 模拟登陆凭证
        /// </summary>
        public string ImpersonationToken { get; set; }

        /// <summary>
        /// 租户系统名称
        /// </summary>
        public string TenancyName { get; set; }
    }
}