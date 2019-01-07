namespace Vapps.Configuration.Host.Dto
{
    /// <summary>
    /// �⻧��������
    /// </summary>
    public class TenantManagementSettingsEditDto
    {
        /// <summary>
        /// �����⻧ע��
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// ��ע���⻧Ĭ�ϼ���
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        /// <summary>
        /// ����(ͼ��)��֤��
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }

        /// <summary>
        /// Ĭ�ϰ汾Id
        /// </summary>
        public int? DefaultEditionId { get; set; }
    }
}