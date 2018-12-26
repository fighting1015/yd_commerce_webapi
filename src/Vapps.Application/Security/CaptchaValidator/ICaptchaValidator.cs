using System.Threading.Tasks;

namespace Vapps.Security.Recaptcha
{
    public interface ICaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}