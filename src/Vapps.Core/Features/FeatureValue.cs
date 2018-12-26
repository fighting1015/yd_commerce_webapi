using Abp;
using System.Collections.Generic;

namespace Vapps.Features
{
    public class FeatureValue : NameValue
    {
        public FeatureValue()
        {

        }

        public FeatureValue(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 版本功能
        /// </summary>
        public List<NameValue> Childs { get; set; }
    }
}
