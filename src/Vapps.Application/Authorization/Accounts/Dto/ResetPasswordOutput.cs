namespace Vapps.Authorization.Accounts.Dto
{
    public class ResetPasswordOutput
    {
        /// <summary>
        /// 能否登陆
        /// </summary>
        public bool CanLogin { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}