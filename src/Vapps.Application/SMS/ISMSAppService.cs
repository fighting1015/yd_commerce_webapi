using System.Threading.Tasks;
using Vapps.Identity;
using Vapps.SMS.Dto;

namespace Vapps.SMS
{
    public interface ISMSAppService
    {
        /// <summary>
        /// 发送(通知/内容)短信 (暂未实现)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SendResult> BatchSend(BatchSendSMSInput input);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SendResult> SendCode(CodeSendInput input);

        /// <summary>
        /// 给当前用户发送验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SendResult> SendCodeByCurrentUser(UserCodeSendInput input);

        /// <summary>
        /// 验证当前用户的验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CheckCodeByCurrentUser(CheckUserCodeInput input);
    }
}
