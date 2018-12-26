using Abp.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Security.Recaptcha;

namespace Vapps.Web.Security.CaptchaValidator
{
    public class LuosimaoCaptchaValidator : VappsServiceBase, ICaptchaValidator
    {
        private const string CAPTCHA_VERIFY_URL_LUOSIMAO = "https://captcha.luosimao.com/api/site_verify?api_key={0}&response={1}";

        private string _secretKey { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LuosimaoCaptchaValidator(IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment env)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._env = env;
            this._appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
            this._secretKey = _appConfiguration["LuosimaoCaptcha:SecretKey"];
        }

        public async Task ValidateAsync(string captchaResponse)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new Exception("RecaptchaValidator should be used in a valid HTTP context!");
            }

            if (captchaResponse.IsNullOrEmpty())
            {
                throw new UserFriendlyException(L("CaptchaCanNotBeEmpty"));
            }

            var result = await Validate(captchaResponse);
            if (!result.IsValid)
                throw new UserFriendlyException(L("IncorrectCaptchaAnswer"));
        }

        public async Task<CaptchaResponse> Validate(string captchaResponse)
        {
            CaptchaResponse result = null;
            var httpClient = new HttpClient();

            var requestUri = string.Format(CAPTCHA_VERIFY_URL_LUOSIMAO, _secretKey, captchaResponse);

            try
            {
                var taskResult = httpClient.GetAsync(requestUri);
                taskResult.Wait();
                var response = taskResult.Result;
                response.EnsureSuccessStatusCode();
                var taskString = await response.Content.ReadAsStringAsync();
                result = ParseResponseResult(taskString);
            }
            catch
            {
                result = new CaptchaResponse { IsValid = false };
                result.ErrorMsg = "Unknown error";
            }
            finally
            {
                httpClient.Dispose();
            }

            return result;
        }

        private CaptchaResponse ParseResponseResult(string responseString)
        {
            var result = new CaptchaResponse();

            var resultObject = JObject.Parse(responseString);
            result.IsValid = resultObject.Value<string>("res") == "success";
            result.ErrorCodes = resultObject.Value<int>("error");
            result.ErrorMsg = resultObject.Value<string>("msg");

            return result;
        }
    }


}
