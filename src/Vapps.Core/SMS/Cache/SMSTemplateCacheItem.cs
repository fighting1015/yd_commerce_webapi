using Abp.AutoMapper;
using System.Collections.Generic;

namespace Vapps.SMS.Cache
{
    [AutoMapFrom(typeof(SMSTemplate))]
    public class SMSTemplateCacheItem
    {
        public const string CacheName = "SMSTemplateCache";

        public int Id { get; set; }

        public string Name { get; set; }

        public string TemplateCode { get; set; }

        public string SmsProvider { get; set; }

        public bool IsActive { get; set; }

        public virtual List<SMSTemplateItemCache> Items { get; set; }
    }

    [AutoMapFrom(typeof(SMSTemplateItem))]
    public class SMSTemplateItemCache
    {
        public string DataItemName { get; set; }

        public string DataItemValue { get; set; }
    }
}
