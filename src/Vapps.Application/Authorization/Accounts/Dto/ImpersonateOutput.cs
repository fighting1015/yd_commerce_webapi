namespace Vapps.Authorization.Accounts.Dto
{
    public class ImpersonateOutput
    {
        /// <summary>
        /// ģ���½ƾ֤
        /// </summary>
        public string ImpersonationToken { get; set; }

        /// <summary>
        /// �⻧ϵͳ����
        /// </summary>
        public string TenancyName { get; set; }
    }
}