using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.WeChat.Core.TemplateMessages
{
    [Table("WeChatTemplateMessages")]
    public class TemplateMessage : FullAuditedEntity, IPassivable
    {
        public const int MaxNameFieldLength = 24;

        /// <summary>
        /// 模板消息名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模板消息Id（微信模板消息Id）
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式 (TemplateId和TemplateIdShort二选一填写)
        /// </summary>
        public string TemplateIdShort { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 标题内容
        /// </summary>
        public string FirstData { get; set; }

        /// <summary>
        /// 标题内容
        /// </summary>
        public string FirstDataColor { get; set; }

        /// <summary>
        /// 备注内容
        /// </summary>
        public string RemarkData { get; set; }

        /// <summary>
        /// 标题内容
        /// </summary>
        public string RemarkDataColor { get; set; }

        /// <summary>
        /// 模板消息跳转url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模板消息子项
        /// </summary>
        [ForeignKey("TemplateMessageId")]
        public virtual ICollection<TemplateMessageItem> MessageItems { get; set; }
    }
}
