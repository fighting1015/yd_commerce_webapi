using Abp.Dependency;
using Abp.Localization;
using System;
using System.ComponentModel.DataAnnotations;
namespace Vapps.Attributes
{
    /// <summary>
    /// 手机号码校验
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PhoneNumberAttribute : ValidationAttribute
    {
        private bool AllowNull { get; set; }
        private bool IsMobile { get; set; }

        public PhoneNumberAttribute(bool allowNull = true, bool isMobile = true)
        {
            this.AllowNull = allowNull;
            this.IsMobile = isMobile;
        }

        public override bool IsValid(object value)
        {
            if (AllowNull && (value == null || value.ToString().IsNullOrEmpty()))
                return true;

            if (value == null)
                return false;

            var localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
            if (IsMobile)
            {
                this.ErrorMessage = localizationManager.GetString("Vapps", "Identity.InvaildPhoneNumber");
                return value.ToString().IsMobilePhone();
            }
            else
            {
                this.ErrorMessage = localizationManager.GetString("Vapps", "Identity.InvaildTelephoneNumber");
                return value.ToString().IsTelephone();
            }
        }

        //public override string FormatErrorMessage(string name)
        //{
        //    var localizationManager = IocManager.Instance.Resolve<ILocalizationManager>();
        //    return localizationManager.GetString("Vapps", "Identity.InvaildPhoneNumber");
        //}
    }
}
