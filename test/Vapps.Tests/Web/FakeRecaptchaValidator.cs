using System.Threading.Tasks;
using Vapps.Security.Recaptcha;

namespace Vapps.Tests.Web
{
    public class FakeRecaptchaValidator : ICaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
