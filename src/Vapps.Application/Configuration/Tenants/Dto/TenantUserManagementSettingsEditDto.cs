namespace Vapps.Configuration.Tenants.Dto
{
    public class TenantUserManagementSettingsEditDto
    {
        /// <summary>
        /// �����⻧ע��
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// ��ע���û�Ĭ�ϼ���
        /// </summary>
        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        /// <summary>
        /// ��¼ǰ������ȷ��
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        /// <summary>
        /// ����(ͼ��)��֤��
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }
    }
}