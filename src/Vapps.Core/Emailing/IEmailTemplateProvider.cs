namespace Vapps.Emailing
{
    public interface IEmailTemplateProvider
    {
        string GetDefaultTemplate(int? tenantId);

        string GetActiveLinkTemplate();

        string GetCodeVerificationTemplate();
    }
}
