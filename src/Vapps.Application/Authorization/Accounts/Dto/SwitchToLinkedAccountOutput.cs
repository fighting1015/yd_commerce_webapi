namespace Vapps.Authorization.Accounts.Dto
{
    public class SwitchToLinkedAccountOutput
    {
        /// <summary>
        /// �л��˻�����
        /// </summary>
        public string SwitchAccountToken { get; set; }

        /// <summary>
        /// �⻧ϵͳ����
        /// </summary>
        public string TenancyName { get; set; }
    }
}