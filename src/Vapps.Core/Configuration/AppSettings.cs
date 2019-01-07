namespace Vapps.Configuration
{
    /// <summary>
    /// Defines string constants for setting names in the application.
    /// See <see cref="AppSettingProvider"/> for setting definitions.
    /// </summary>
    public static class AppSettings
    {
        public static class HostManagement
        {
        }


        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
            public const string SubscriptionExpireNotifyDayCount = "App.TenantManagement.SubscriptionExpireNotifyDayCount";
            public const string BillingLegalName = "App.UserManagement.BillingLegalName";
            public const string BillingAddress = "App.UserManagement.BillingAddress";
            public const string BillingTaxVatNo = "App.UserManagement.BillingTaxVatNo";
        }

        public static class UserManagement
        {
            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";

            public static class VerificationCodeManagement
            {
                public const string IsEnabled = "App.VerificationCodeManagement.IsEnabled";

                public const string AvailableSecond = "App.VerificationCodeManagement.AvailableSecond";

                public const string MinimumSendInterval = "App.VerificationCodeManagement.MinimumSendInterval";
            }
        }

        public static class ExternalAuthentication
        {
            public const string UserActivation = "App.ExternalAuthentication.UserActivation";

            public const string ProviderName = "App.ExternalAuthentication.Provider.{0}";

            public static class UserActivationCondition
            {
                public const string RequiredUserName = "App.ExternalAuthentication.UserActivationCondition.RequiredUserName";

                public const string RequiredEmail = "App.ExternalAuthentication.UserActivationCondition.RequiredEmail";

                public const string RequiredTelephone = "App.ExternalAuthentication.UserActivationCondition.RequiredTelephone";

                public const string UseTelephoneforUsername = "App.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername";
            }
        }

        public static class SMSManagement
        {
            public const string SmsVerificationEnabled = "App.UserManagement.SmsVerificationEnabled";

            public const string UseCaptchaToVerification = "App.SMSManagement.UseCaptchaToVerification";

            public const string RegisterVerificationTempId = "App.SMSManagement.RegisterVerification";

            public const string ChangePasswordVerificationTempId = "App.SMSManagement.ChangePasswordVerification";

            public const string BindingPhoneVerificationTempId = "App.SMSManagement.BindingPhoneVerification";

            public const string UnBindingPhoneVerificationTempId = "App.SMSManagement.UnBindingPhoneVerification";

            public const string LoginVerificationTempId = "App.SMSManagement.LoginVerification";

            public const string PhoneVerificationTempId = "App.SMSManagement.PhoneVerification";
        }

        public static class Recaptcha
        {
            public const string SiteKey = "Recaptcha.SiteKey";
        }

        public static class CacheKeys
        {
            public const string TenantRegistrationCache = "TenantRegistrationCache";
        }

        public static class UiManagement
        {
            public const string LayoutType = "App.UiManagement.LayoutType";

            public const string ContentSkin = "App.UiManagement.ContentSkin";

            public const string Theme = "App.UiManagement.Theme";

            public static class Header
            {
                public const string DesktopFixedHeader = "App.UiManagement.Header.DesktopFixedHeader";
                public const string DesktopMinimizeMode = "App.UiManagement.Header.DesktopMinimizeMode";
                public const string MobileFixedHeader = "App.UiManagement.Header.MobileFixedHeader";
                public const string Skin = "App.UiManagement.Header.Skin";
                public const string DisplaySubmenuArrowDesktop = "App.UiManagement.Header.DisplaySubmenuArrow_Desktop";
            }

            public static class LeftAside
            {
                public const string Position = "App.UiManagement.Left.Position";
                public const string AsideSkin = "App.UiManagement.Left.AsideSkin";
                public const string FixedAside = "App.UiManagement.Left.FixedAside";
                public const string AllowAsideMinimizing = "App.UiManagement.Left.AllowAsideMinimizing";
                public const string DefaultMinimizedAside = "App.UiManagement.Left.DefaultMinimizedAside";
                public const string AllowAsideHiding = "App.UiManagement.Left.AllowAsideHiding";
                public const string DefaultHiddenAside = "App.UiManagement.Left.DefaultHiddenAside";
                public const string DropdownSubmenuSkin = "App.UiManagement.Left.DropdownSubmenuSkin";
                public const string DropdownSubmenuArrow = "App.UiManagement.Left.DropdownSubmenuArrow";
            }

            public static class Footer
            {
                public const string FixedFooter = "App.UiManagement.Footer.FixedFooter";
            }

        }
    }
}