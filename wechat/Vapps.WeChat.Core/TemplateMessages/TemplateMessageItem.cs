using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.WeChat.Core.TemplateMessages
{
    [Table("WeChatTemplateMessageItems")]
    public class TemplateMessageItem : Entity
    {
        /// <summary>
        /// 模板消息字段名
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 模板消息字段值类
        /// </summary>
        public string DataValue { get; set; }

        /// <summary>
        /// 模板消息字段颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 模板消息Id
        /// </summary>
        public int TemplateMessageId { get; set; }
    }
}
