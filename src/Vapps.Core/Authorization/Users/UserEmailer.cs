using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vapps.Configuration;
using Vapps.Editions;
using Vapps.Emailing;
using Vapps.Identity;
using Vapps.Localization;
using Vapps.MultiTenancy;

namespace Vapps.Authorization.Users
{
    /// <summary>
    /// Used to send email to users.
    /// </summary>
    public class UserEmailer : VappsServiceBase, IUserEmailer, ITransientDependency
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISettingManager _settingManager;
        private readonly EditionManager _editionManager;

        public UserEmailer(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            ISettingManager settingManager, EditionManager editionManager)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository = userRepository;
            _settingManager = settingManager;
            _editionManager = editionManager;
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        /// <summary>
        /// 发送邮箱激活邮件
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="link">激活链接</param>
        /// <param name="plainPassword">密码明文 </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new Exception("EmailConfirmationCode should be set in order to send email activation link.");
            }

            link = link.Replace("{userId}", user.Id.ToString());
            link = link.Replace("{confirmationCode}", Uri.EscapeDataString(user.EmailConfirmationCode));

            if (user.TenantId.HasValue)
            {
                link = link.Replace("{tenantId}", user.TenantId.ToString());
            }

            link = EncryptQueryParameters(link);

            var mailMessage = GetActiveLinkEmailTemplate();
            mailMessage = mailMessage.Replace("{ACTIVE_LINK}", link);

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailTemplate.Activation"), mailMessage);
        }

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="emailAddress">邮箱地址</param>
        /// <param name="code">验证码</param>
        /// <param name="codeType">验证码类型</param>
        [UnitOfWork]
        public virtual async Task SendEmailVerificationCodeAsync(string emailAddress, string code, VerificationCodeType codeType)
        {
            if (code.IsNullOrEmpty())
            {
                throw new Exception("code should be set in order to send email code.");
            }
            string subject = string.Empty;
            StringBuilder mailMessage = new StringBuilder();

            switch (codeType)
            {
                case VerificationCodeType.EmailBinding:
                    {
                        subject = L("EmailTemplate.EmailVerification");
                        mailMessage = GetCodeVerificationEmailTemplate(L("EmailTemplate.EmailVerification"), L("EmailTemplate.EmailVerification"), L("EmailTemplate.EmailBindingSlogen"));
                        break;
                    }
                case VerificationCodeType.EmailUnBinding:
                    {
                        subject = L("EmailTemplate.ChangeEmail");
                        mailMessage = GetCodeVerificationEmailTemplate(L("EmailTemplate.ChangeEmail"), L("EmailTemplate.ChangeEmail"), L("EmailTemplate.EmailBindingSlogen"));
                        break;
                    }
                case VerificationCodeType.ChangePassword:
                    {
                        subject = L("EmailTemplate.ChangeEmail");
                        mailMessage = GetCodeVerificationEmailTemplate(L("EmailTemplate.ChangeEmail"), L("EmailTemplate.ChangeEmail"), L("EmailTemplate.EmailBindingSlogen"));
                        break;
                    }
                case VerificationCodeType.Register:
                default:
                    {
                        subject = L("EmailTemplate.Register");
                        mailMessage = GetCodeVerificationEmailTemplate(L("EmailTemplate.Register"), L("EmailTemplate.Register"), L("EmailTemplate.RegisterSlogen"));
                        break;
                    }
            }

            mailMessage = mailMessage.Replace("{CODE}", code);
            await ReplaceBodyAndSend(emailAddress, subject, mailMessage);
        }

        /// <summary>
        /// 发送密码重置邮件.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Reset link</param>
        public async Task SendPasswordResetLinkAsync(User user, string link = null)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new Exception("PasswordResetCode should be set in order to send password reset link.");
            }

            if (!link.IsNullOrEmpty())
            {
                link = link.Replace("{userId}", user.Id.ToString());
                link = link.Replace("{resetCode}", Uri.EscapeDataString(user.PasswordResetCode));

                if (user.TenantId.HasValue)
                {
                    link = link.Replace("{tenantId}", user.TenantId.ToString());
                }
            }

            link = EncryptQueryParameters(link);

            var mailMessage = GetPasswordResetEmailTemplate();
            mailMessage = mailMessage.Replace("{ACTIVE_LINK}", link);

            await ReplaceBodyAndSend(user.EmailAddress, L("EmailTemplate.PasswordReset.Title"), mailMessage);
        }

        /// <summary>
        /// 发送订阅到期邮件
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="utcNow"></param>
        public async void TryToSendSubscriptionExpireEmail(int tenantId, DateTime utcNow)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = _userRepository.FirstOrDefault(u => u.UserName == AbpUserBase.AdminUserName);
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpire_Email_Body", culture, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        /// <summary>
        /// 发送版本过期邮件
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="utcNow"></param>
        /// <param name="expiringEditionId"></param>
        /// <returns></returns>
        public async Task TryToSendSubscriptionAssignedToAnotherEmail(int tenantId, DateTime utcNow, int expiringEditionId)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = _userRepository.FirstOrDefault(u => u.UserName == AbpUserBase.AdminUserName);
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                        var expringEdition = await _editionManager.GetByIdAsync(expiringEditionId);
                        var emailTemplate = GetTitleAndSubTitle(tenantId, L("SubscriptionExpire_Title"), L("SubscriptionExpire_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionAssignedToAnother_Email_Body", culture, expringEdition.DisplayName, utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpire_Email_Subject"), mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        /// <summary>
        /// 终止订阅失败邮件
        /// </summary>
        /// <param name="failedTenancyNames"></param>
        /// <param name="utcNow"></param>
        public async void TryToSendFailedSubscriptionTerminationsEmail(List<string> failedTenancyNames, DateTime utcNow)
        {
            try
            {
                var hostAdmin = _userRepository.FirstOrDefault(u => u.UserName == AbpUserBase.AdminUserName);
                if (hostAdmin == null || string.IsNullOrEmpty(hostAdmin.EmailAddress))
                {
                    return;
                }

                var hostAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, hostAdmin.TenantId, hostAdmin.Id);
                var culture = CultureHelper.GetCultureInfoByChecking(hostAdminLanguage);
                var emailTemplate = GetTitleAndSubTitle(null, L("FailedSubscriptionTerminations_Title"), L("FailedSubscriptionTerminations_SubTitle"));
                var mailMessage = new StringBuilder();

                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("FailedSubscriptionTerminations_Email_Body", culture, string.Join(",", failedTenancyNames), utcNow.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                mailMessage.AppendLine("<br />");

                await ReplaceBodyAndSend(hostAdmin.EmailAddress, L("FailedSubscriptionTerminations_Email_Subject"), mailMessage);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        /// <summary>
        /// 订阅即将过期通知
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="dateToCheckRemainingDayCount"></param>
        public async void TryToSendSubscriptionExpiringSoonEmail(int tenantId, DateTime dateToCheckRemainingDayCount)
        {
            try
            {
                using (_unitOfWorkManager.Begin())
                {
                    using (_unitOfWorkManager.Current.SetTenantId(tenantId))
                    {
                        var tenantAdmin = _userRepository.FirstOrDefault(u => u.UserName == AbpUserBase.AdminUserName);
                        if (tenantAdmin == null || string.IsNullOrEmpty(tenantAdmin.EmailAddress))
                        {
                            return;
                        }

                        var tenantAdminLanguage = _settingManager.GetSettingValueForUser(LocalizationSettingNames.DefaultLanguage, tenantAdmin.TenantId, tenantAdmin.Id);
                        var culture = CultureHelper.GetCultureInfoByChecking(tenantAdminLanguage);

                        var emailTemplate = GetTitleAndSubTitle(null, L("SubscriptionExpiringSoon_Title"), L("SubscriptionExpiringSoon_SubTitle"));
                        var mailMessage = new StringBuilder();

                        mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + L("SubscriptionExpiringSoon_Email_Body", culture, dateToCheckRemainingDayCount.ToString("yyyy-MM-dd") + " UTC") + "<br />");
                        mailMessage.AppendLine("<br />");

                        await ReplaceBodyAndSend(tenantAdmin.EmailAddress, L("SubscriptionExpiringSoon_Email_Subject"), mailMessage);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }

        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }

        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private StringBuilder GetActiveLinkEmailTemplate()
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetActiveLinkTemplate());

            emailTemplate.Replace("{PRODUCT_NAME}", L("Vapps"));
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailTemplate.EmailVerification"));
            emailTemplate.Replace("{EMAIL_SLOGAN}", L("EmailTemplate.Slogan"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EmailTemplate.EmailVerification"));
            emailTemplate.Replace("{EMIAL_CONTEXT_SLOGEN}", L("EmailTemplate.RegisterSlogen"));
            emailTemplate.Replace("{CLICK_2_VERIFICATION}", L("EmailTemplate.Click2Verification"));
            emailTemplate.Replace("{VERIFICATION_BUTTON}", L("EmailTemplate.VerificationButton"));
            emailTemplate.Replace("{COPY_2_BROWSER}", L("EmailTemplate.Copy2Browser"));
            emailTemplate.Replace("{EXPIRATION_TIME}", string.Format(L("EmailTemplate.ActiveLink.ExpirationTime"), 24));
            emailTemplate.Replace("{IGNORE}", L("EmailTemplate.Ignore"));
            emailTemplate.Replace("{SUBSCRIBE_WECHATMP}", L("EmailTemplate.SubscribeWeChatMp"));
            emailTemplate.Replace("{CONSULT}", L("EmailTemplate.Consult"));
            emailTemplate.Replace("{OFFICIAL_MAILBOX}", L("EmailTemplate.OfficialMailbox"));
            emailTemplate.Replace("{SERVICE_TEL}", L("EmailTemplate.ServiceTel"));
            emailTemplate.Replace("{COPY_RIGHT}", L("EmailTemplate.Copyright"));
            return emailTemplate;
        }

        private StringBuilder GetPasswordResetEmailTemplate()
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetActiveLinkTemplate());

            emailTemplate.Replace("{PRODUCT_NAME}", L("Vapps"));
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailTemplate.PasswordReset.Title"));
            emailTemplate.Replace("{EMAIL_SLOGAN}", L("EmailTemplate.Slogan"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EmailTemplate.PasswordReset.Title"));
            emailTemplate.Replace("{EMIAL_CONTEXT_SLOGEN}", L("EmailTemplate.PasswordReset.Slogen"));
            emailTemplate.Replace("{CLICK_2_VERIFICATION}", L("EmailTemplate.PasswordReset.Click2Reset"));
            emailTemplate.Replace("{VERIFICATION_BUTTON}", L("EmailTemplate.PasswordReset.Button"));
            emailTemplate.Replace("{COPY_2_BROWSER}", L("EmailTemplate.Copy2Browser"));
            emailTemplate.Replace("{EXPIRATION_TIME}", string.Format(L("EmailTemplate.PasswordReset.ExpirationTime"), 24));
            emailTemplate.Replace("{IGNORE}", L("EmailTemplate.Ignore"));
            emailTemplate.Replace("{SUBSCRIBE_WECHATMP}", L("EmailTemplate.SubscribeWeChatMp"));
            emailTemplate.Replace("{CONSULT}", L("EmailTemplate.Consult"));
            emailTemplate.Replace("{OFFICIAL_MAILBOX}", L("EmailTemplate.OfficialMailbox"));
            emailTemplate.Replace("{SERVICE_TEL}", L("EmailTemplate.ServiceTel"));
            emailTemplate.Replace("{COPY_RIGHT}", L("EmailTemplate.Copyright"));
            return emailTemplate;
        }


        private StringBuilder GetCodeVerificationEmailTemplate(string title, string subTitle, string contextSlogen)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetCodeVerificationTemplate());
            var availableSecond = _settingManager.GetSettingValue<int>(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond);
            emailTemplate.Replace("{PRODUCT_NAME}", L("Vapps"));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SLOGAN}", L("EmailTemplate.Slogan"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);
            emailTemplate.Replace("{EMIAL_CONTEXT_SLOGEN}", contextSlogen);
            emailTemplate.Replace("{CODE_TIP}", L("EmailTemplate.CodeTip"));
            emailTemplate.Replace("{EXPIRATION_TIME}", string.Format(L("EmailTemplate.VerificationCode.ExpirationTime"), decimal.Round(availableSecond / 60), 0));
            emailTemplate.Replace("{IGNORE}", L("EmailTemplate.Ignore"));
            emailTemplate.Replace("{SUBSCRIBE_WECHATMP}", L("EmailTemplate.SubscribeWeChatMp"));
            emailTemplate.Replace("{CONSULT}", L("EmailTemplate.Consult"));
            emailTemplate.Replace("{OFFICIAL_MAILBOX}", L("EmailTemplate.OfficialMailbox"));
            emailTemplate.Replace("{SERVICE_TEL}", L("EmailTemplate.ServiceTel"));
            emailTemplate.Replace("{COPY_RIGHT}", L("EmailTemplate.Copyright"));
            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder mailMessage)
        {
            if (emailAddress.IsNullOrWhiteSpace()) return;

            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = mailMessage.ToString(),
                IsBodyHtml = true
            });
        }

        /// <summary>
        /// Returns link with encrypted parameters
        /// </summary>
        /// <param name="link"></param>
        /// <param name="encrptedParameterName"></param>
        /// <returns></returns>
        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var uri = new Uri(link);
            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = uri.Query.TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }
    }
}