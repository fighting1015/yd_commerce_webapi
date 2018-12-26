using Abp.AutoMapper;
using System;
using System.Collections.Generic;

namespace Vapps.SMS.Dto
{
    public class CreateOrUpdateSMSTemplateInput
    {
        public CreateOrUpdateSMSTemplateInput()
        {
            Items = new List<SMSTemplateItemInput>();
        }

        /// <summary>
        /// 模板Id，null时为创建,有值时为更新
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 模板消息名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短息模板编号（第三方短信编号）
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 短信供应商名称
        /// </summary>
        public string SmsProvider { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 模板参数集合
        /// </summary>
        public List<SMSTemplateItemInput> Items { get; set; }
    }


    [AutoMapFrom(typeof(SMSTemplateItem))]
    public class SMSTemplateItemInput
    {
        /// <summary>
        /// 模板参数Id，null时为创建,有值时为更新
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 模板消息字段名
        /// </summary>
        public string DataItemName { get; set; }

        /// <summary>
        /// 模板消息字段值类
        /// </summary>
        public string DataItemValue { get; set; }
    }

}
