using Abp.Dependency;
using Abp.Localization;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Attributes
{
    /// <summary>
    /// 可空邮箱地址校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class EmailAddressAttribute : DataTypeAttribute
    {
        private bool AllowNull { get; set; }

        public EmailAddressAttribute(bool allowNull) : base(DataType.PhoneNumber)
        {
            this.AllowNull = allowNull;
            var localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            this.ErrorMessage = localizationManager.GetString("Vapps", "Identity.InvaildEmail");
        }

        public override bool IsValid(object value)
        {
            if (AllowNull && (value == null || value.ToString().IsNullOrEmpty()))
                return true;

            if (value == null)
                return false;

            return value.ToString().IsEmail();
        }
    }
}
