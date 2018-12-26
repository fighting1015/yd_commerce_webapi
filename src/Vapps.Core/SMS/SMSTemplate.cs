using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.SMS
{
    [Table("SMSTemplate")]
    public class SMSTemplate : FullAuditedEntity<long>
    {
        public const int MaxFieldLength = 256;

        /// <summary>
        /// Items of this SMS template.
        /// </summary>
        [ForeignKey("TemplateMessageId")]
        public virtual ICollection<SMSTemplateItem> Items { get; set; }

        /// <summary>
        /// 模板消息名称
        /// </summary>
        [Required]
        [StringLength(MaxFieldLength)]
        public string Name { get; set; }

        /// <summary>
        /// 短息模板编号（第三方短信编号）
        /// </summary>
        [StringLength(MaxFieldLength)]
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短信供应商名称
        /// </summary>
        [StringLength(MaxFieldLength)]
        public string SmsProvider { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
