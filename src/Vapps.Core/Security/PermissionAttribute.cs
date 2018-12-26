using Abp.MultiTenancy;
using System;

namespace Vapps.Security
{
    /// <summary>
    /// 权限属性
    /// </summary>
    public class PermissionAttribute : Attribute
    {
        /// <summary>
        /// 作用范围
        /// </summary>
        private MultiTenancySides _multiTenancySides { get; set; }

        /// <summary>
        /// 是否依赖于启用多租户
        /// </summary>
        private bool _dependOnEnableMultiTenancy { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        private string _description { get; set; }

        public PermissionAttribute(string description,
            MultiTenancySides multiTenancySides = MultiTenancySides.Tenant | MultiTenancySides.Host,
            bool dependOnEnableMultiTenancy = false)
        {
            this._description = description;
            this._multiTenancySides = multiTenancySides;
            this._dependOnEnableMultiTenancy = dependOnEnableMultiTenancy;
        }

        public MultiTenancySides MultiTenancySides
        {
            get { return _multiTenancySides; }
            set { this._multiTenancySides = value; }
        }

        public bool DependOnEnableMultiTenancy
        {
            get { return _dependOnEnableMultiTenancy; }
            set { this._dependOnEnableMultiTenancy = value; }
        }

        public string Description
        {
            get { return _description; }
            set { this._description = value; }
        }
    }
}
