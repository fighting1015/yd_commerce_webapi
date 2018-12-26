using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    [AutoMapFrom(typeof(SMSTemplateCacheItem))]
    public class SendSMSTemplateResult
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TemplateCode { get; set; }

        public string SmsProvider { get; set; }

        public bool IsActive { get; set; }

        public virtual List<SendSMSTemplateItemResult> Items { get; set; }
    }

    [AutoMapFrom(typeof(SMSTemplateItemCache))]
    public class SendSMSTemplateItemResult
    {
        public string DataItemName { get; set; }

        public string DataItemValue { get; set; }
    }
}
