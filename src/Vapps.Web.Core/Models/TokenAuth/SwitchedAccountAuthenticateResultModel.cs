namespace Vapps.Web.Models.TokenAuth
{
    /// <summary>
    /// �л��˺���֤ - ���
    /// </summary>
    public class SwitchedAccountAuthenticateResultModel
    {
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
    }
}