namespace Vapps.Authorization.Accounts.Dto
{
    public class RegisterOutput
    {
        /// <summary>
        /// �ܷ��½
        /// </summary>
        public bool CanLogin { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string RandomPassword { get; set; }
    }
}