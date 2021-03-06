﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class JsonProductAttribute
    {
        public JsonProductAttribute() {
            this.AttributeValues = new List<JsonProductAttributeValue>();
        }

        /// <summary>
        /// 属性Id
        /// </summary>
        public long AttributeId { get; set; }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public List<JsonProductAttributeValue> AttributeValues { get; set; }
    }

    /// <summary>
    /// 商品属性
    /// </summary>
    public class JsonProductAttributeValue
    {
        /// <summary>
        /// 属性值Id
        /// </summary>
        public long AttributeValueId { get; set; }
    }
}
