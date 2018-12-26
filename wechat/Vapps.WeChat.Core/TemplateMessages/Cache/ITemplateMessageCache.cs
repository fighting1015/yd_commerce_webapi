namespace Vapps.WeChat.Core.TemplateMessages.Cache
{
    public interface ITemplateMessageCache
    {
        TemplateMessageCacheItem Get(int id);

        TemplateMessageCacheItem GetOrNull(int id);
    }
}
