namespace Vapps.SMS.Cache
{
    public interface ISMSTemplateCache
    {
        SMSTemplateCacheItem Get(long id);

        SMSTemplateCacheItem GetOrNull(long id);
    }
}
