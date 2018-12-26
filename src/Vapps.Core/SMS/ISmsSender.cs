using System.Threading.Tasks;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    public interface ISmsSender
    {
        Task<SendResult> SendAsync(string[] targetNumbers, string content, string provider = "Alidayu");

        Task<SendResult> SendAsync(string targetNumber, SendSMSTemplateResult sms, string provider = "Alidayu");
    }
}