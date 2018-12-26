using System.Threading.Tasks;

namespace Vapps.Security.Recaptcha
{
    public class NullCaptchaValidator : ICaptchaValidator
    {
        public static NullCaptchaValidator Instance { get; } = new NullCaptchaValidator();

        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}