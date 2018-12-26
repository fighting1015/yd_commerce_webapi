using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.SMS
{
    [Table("SMSTemplateItem")]
    public class SMSTemplateItem : Entity<long>, IHasCreationTime
    {
        /// <summary>
        /// 模板消息字段名
        /// </summary>
        public string DataItemName { get; set; }

        /// <summary>
        /// 模板消息字段值类
        /// </summary>
        public string DataItemValue { get; set; }

        /// <summary>
        /// 模板消息Id
        /// </summary>
        public long TemplateMessageId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
