using System.Threading.Tasks;
using Vapps.Identity;

namespace Vapps.Authorization.Users
{
    public interface IUserEmailer
    {
        /// <summary>
        /// Send email activation link to user's email address.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Email activation link</param>
        /// <param name="plainPassword">
        /// Can be set to user's plain password to include it in the email.
        /// </param>
        Task SendEmailActivationLinkAsync(User user, string link, string plainPassword = null);

        /// <summary>
        /// Send email verification code to user's email address.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <param name="code">Email verification code</param>
        /// <param name="codeType">Code type</param>
        Task SendEmailVerificationCodeAsync(string emailAddress, string code, VerificationCodeType codeType);

        /// <summary>
        /// Sends a password reset link to user's email.
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="link">Password reset link (optional)</param>
        Task SendPasswordResetLinkAsync(User user, string link = null);
    }
}
