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
        /// ��ע��
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        public bool UseCaptchaOnRegistration { get; set; }

        public int? DefaultEditionId { get; set; }

    }
}