using System.Collections.Generic;

namespace Vapps.WeChat.Core.TemplateMessages.Cache
{
    public class TemplateMessageCacheItem
    {
        public TemplateMessageCacheItem()
        {
            Items = new List<TemplateMessageItemCacheItem>();
        }

        public const string CacheName = "TemplateMessageCache";

        public int Id { get; set; }

        public string Name { get; set; }

        public string TemplateId { get; set; }

        public string TemplateIdShort { get; set; }

        public string RGBColor { get; set; }

        public bool IsActive { get; set; }

        public string FirstData { get; set; }

        public string FirstDataColor { get; set; }

        public string RemarkData { get; set; }

        public string RemarkDataColor { get; set; }

        public string Url { get; set; }

        public List<TemplateMessageItemCacheItem> Items { get; set; }
    }

    public class TemplateMessageItemCacheItem
    {
        public int Id { get; set; }

        public string DataName { get; set; }

        public string DataValue { get; set; }

        public string Color { get; set; }
    }
}
