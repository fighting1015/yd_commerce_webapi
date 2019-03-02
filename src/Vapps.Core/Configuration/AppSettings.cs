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

        public static class Shipping
        {
            public const string ApiId = "Shipping.ApiId";

            public const string ApiSecret = "Shipping.ApiSecret";

            public const string ApiUrl = "Shipping.ApiUrl";
        }
    }
}