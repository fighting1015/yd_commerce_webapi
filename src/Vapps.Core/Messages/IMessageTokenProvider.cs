using System.Collections.Generic;
using Vapps.Authorization.Users;
using Vapps.Identity.Cache;

namespace Vapps.Messages
{
    public interface IMessageTokenProvider
    {
        /// <summary>
        /// 替换指令
        /// </summary>
        /// <param name="templateWithToken"></param>
        /// <param name="user"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        string ReplaceToken(string templateWithToken,
           User user = null,
           VerificationCodeCacheItem verificationCode = null);

        /// <summary>
        /// 获取所有可用指令
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAllowedTokens();

        /// <summary>
        /// 用户指令 TODO:待补充
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="user"></param>
        /// <param name="authProviderName"></param>
        void AddUserTokens(IList<Token> tokens, User user, string authProviderName = "");

        /// <summary>
        /// 验证码指令
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="code"></param>
        void AddVerificationCodeTokens(IList<Token> tokens, VerificationCodeCacheItem code);

        /// <summary>
        /// 用户指令
        /// </summary>
        /// <returns></returns>
        string[] GetListOfUserAllowedTokens();

        /// <summary>
        /// 验证码指令
        /// </summary>
        /// <returns></returns>
        string[] GetListOfVerificationCodeAllowedTokens();
    }
}
