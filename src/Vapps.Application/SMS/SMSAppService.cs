using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.UI;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.Identity;
using Vapps.Security.Recaptcha;
using Vapps.SMS.Dto;

namespace Vapps.SMS
{
    public class SMSAppService : VappsAppServiceBase, ISMSAppService
    {
        private readonly UserStore _userStore;
        private readonly ISmsSender _smsSender;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IVerificationCodeManager _verificationCodeManager;

        private readonly ISMSTemplateManager _smsTemplateManager;

        public SMSAppService(UserStore userStore,
            ISmsSender smsSender,
            ICaptchaValidator captchaValidator,
            IVerificationCodeManager verificationCodeManager,
            ISMSTemplateManager smsTemplateManager)
        {
            this._userStore = userStore;
            this._smsSender = smsSender;
            this._captchaValidator = captchaValidator;
            this._verificationCodeManager = verificationCodeManager;
            this._smsTemplateManager = smsTemplateManager;
        }

        /// <summary>
        /// 发送(通知/内容)短信 (暂未实现)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SendResult> BatchSend(BatchSendSMSInput input)
        {
            //await _captchaValidator.ValidateAsync(input.CaptchaResponse);

            var result = await _smsSender.SendAsync(input.TargetNumbers, input.Content);

            return result;
        }

        /// <summary>
        /// 给当前用户发送验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<SendResult> SendCodeByCurrentUser(UserCodeSendInput input)
        {
            var user = await GetCurrentUserAsync();
            if (user.PhoneNumber.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.UnBindingPhoneNum"));

            return await SendCode(new CodeSendInput()
            {
                TargetNumber = user.PhoneNumber,
                CaptchaResponse = input.CaptchaResponse,
                CodeType = input.CodeType
            });
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SendResult> SendCode(CodeSendInput input)
        {
            await PreSendCodeCheck(input);

            await CheckPhoneNumVaild(input);
            var verificationCode = await _verificationCodeManager.GetOrSetVerificationCodeAsync(input.TargetNumber, input.CodeType);

            int tempateId = 0;
            switch (input.CodeType)
            {
                case VerificationCodeType.Register:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.RegisterVerificationTempId);
                    break;
                case VerificationCodeType.Login:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.LoginVerificationTempId);
                    break;
                case VerificationCodeType.ChangePassword:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.ChangePasswordVerificationTempId);
                    break;
                case VerificationCodeType.PhoneVerify:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.BindingPhoneVerificationTempId);
                    break;
                case VerificationCodeType.PhoneBinding:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.BindingPhoneVerificationTempId);
                    break;
                case VerificationCodeType.PhoneUnBinding:
                    tempateId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.UnBindingPhoneVerificationTempId);
                    break;
                default:
                    break;
            }

            if (tempateId <= 0)
                return new SendResult() { Success = false };

            var template = _smsTemplateManager.GetSMSTemplateResultById(id: tempateId, verificationCode: verificationCode);
            return await _smsSender.SendAsync(input.TargetNumber, template, template.SmsProvider);
        }

        /// <summary>
        /// 验证当前用户的验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CheckCodeByCurrentUser(CheckUserCodeInput input)
        {
            //验证当前用户的手机验证码
            var user = await GetCurrentUserAsync();
            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.Code, user.PhoneNumber, input.CodeType);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));
        }

        /// <summary>
        /// 验证手机号码是否有效
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckPhoneNumVaild(CodeSendInput input)
        {
            if (input.CodeType == VerificationCodeType.Register)
            {
                var user = await _userStore.Find4PlatformByPhoneNumberAsync(input.TargetNumber);
                if (user != null)
                    throw new UserFriendlyException(string.Format(L("Identity.DuplicatePhoneNumber"), input.TargetNumber));
            }
            else if (input.CodeType == VerificationCodeType.PhoneBinding)
            {
                var currentUser = await GetCurrentUserIfLoginAsync();
                var user = await _userStore.Find4PlatformByPhoneNumberAsync(input.TargetNumber);

                //未登录，不能收取手机绑定验证码
                if (currentUser == null)
                    throw new UserFriendlyException(L("Identity.NotLoggedIn"));

                if (user != null)
                {
                    if (user != currentUser)
                        throw new UserFriendlyException(string.Format(L("Identity.DuplicatePhoneNumber"), input.TargetNumber));

                    if (user == currentUser)
                        throw new UserFriendlyException(string.Format(L("Identity.BindingPhoneNumCanNotEqual"), input.TargetNumber));
                }
            }
            else if (input.CodeType == VerificationCodeType.PhoneUnBinding)
            {
                var currentUser = await GetCurrentUserIfLoginAsync();
                if (currentUser == null || currentUser.PhoneNumber.IsNullOrWhiteSpace())
                {
                    throw new UserFriendlyException(string.Format(L("Identity.UnBindingPhoneNum"), input.TargetNumber));
                }
            }
            else if (input.CodeType == VerificationCodeType.Login)
            {
                var user = await _userStore.Find4PlatformByPhoneNumberAsync(input.TargetNumber);
                if (user == null)
                    throw new UserFriendlyException(L("Identity.UnRegisterPhoneNumber"));
            }
        }

        /// <summary>
        /// 发送验证码前检查
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task PreSendCodeCheck(CodeSendInput input)
        {
            if (SettingManager.GetSettingValueForApplication<bool>(AppSettings.SMSManagement.UseCaptchaToVerification))
                await _captchaValidator.ValidateAsync(input.CaptchaResponse);
        }
    }
}
