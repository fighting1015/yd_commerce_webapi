namespace Vapps.Web.Models.TokenAuth
{
    public class ImpersonatedAuthenticateResultModel
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// ��������(����)
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// ����ʱ��(��)
        /// </summary>
        public int ExpireInSeconds { get; set; }
    }
}