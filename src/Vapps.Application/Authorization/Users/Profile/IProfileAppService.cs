using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Vapps.Authorization.Users.Profile.Dto;
using Vapps.Authorization.Users.Dto;

namespace Vapps.Authorization.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        /// <summary>
        /// 获取当前用户资料
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        /// <summary>
        /// 更新当前用户资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangePassword(ChangePasswordInput input);

        /// <summary>
        /// 使用手机修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangePasswordByPhone(ChangePasswordByPhoneInput input);

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateProfilePicture(UpdateProfilePictureInput input);

        /// <summary>
        /// 获取密码复杂性
        /// </summary>
        /// <returns></returns>
        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();

        /// <summary>
        /// 获取当前用户头像
        /// </summary>
        /// <returns></returns>
        Task<GetProfilePictureOutput> GetProfilePicture();

        /// <summary>
        /// 根据 Id 获取头像
        /// </summary>
        /// <param name="profilePictureId">头像文件Id</param>
        /// <returns></returns>
        Task<GetProfilePictureOutput> GetProfilePictureById(long profilePictureId);

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeLanguage(ChangeUserLanguageDto input);

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindingPhoneNum(BindingPhoneNumInput input);

        /// <summary>
        /// 解绑手机
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task UnBindingPhoneNum(string code);

        /// <summary>
        /// 获取当前用户安全信息
        /// </summary>
        /// <returns></returns>
        Task<UserSecurityInfoDto> GetCurrentUserSecurityInfo();

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BindingEmailAddress(BindingEmailInput input);

        /// <summary>
        /// 修改绑定邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeBindingEmail(ChangeBindingEmailInput input);

        /// <summary>
        /// 解绑邮箱
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task UnBindingEmailAddress(string code);
    }
}
