namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalAuthenticateResultModel
    {
        /// <summary>
        /// �⻧Id 
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// �û�Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// �������ƣ����ܣ�
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// ����ʱ�䣨��λ:�룩
        /// </summary>
        public int ExpireInSeconds { get; set; }

        /// <summary>
        /// �ȴ�����
        /// </summary>
        public bool WaitingForActivation { get; set; }

        /// <summary>
        /// ��Ҫ����ע��
        /// </summary>
        public bool NeedSupplementary { get; set; }
    }
}