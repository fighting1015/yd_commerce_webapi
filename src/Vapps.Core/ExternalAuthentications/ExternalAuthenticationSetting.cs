using Newtonsoft.Json;
using System.Collections.Generic;
using Vapps.Enums;

namespace Vapps.ExternalAuthentications
{
    /// <summary>
    /// 第三方登陆配置
    /// </summary>
    public class ExternalAuthenticationSetting
    {
        public ExternalAuthenticationSetting()
        {
            this.ExternalAuthenticationProviders = new List<ExternalAuthenticationProvider>();
        }

        /// <summary>
        /// 激活账号选项 Id <see cref="UserActivationOption"/>
        /// </summary>
        public int UserActivationId { get; set; }

        [JsonIgnore]
        public UserActivationOption UserActivation
        {
            get { return (UserActivationOption)this.UserActivationId; }
            set { this.UserActivationId = (int)value; }
        }

        /// <summary>
        /// 需要用户名
        /// </summary>
        public bool RequiredUserName { get; set; }

        /// <summary>
        /// 需要邮箱
        /// </summary>
        public bool RequiredEmail { get; set; }

        /// <summary>
        /// 需要手机
        /// </summary>
        public bool RequiredTelephone { get; set; }

        /// <summary>
        /// 使用手机作为用户名
        /// </summary>
        public bool UseTelephoneforUsername { get; set; }

        /// <summary>
        /// 第三方登陆供应商
        /// </summary>
        public List<ExternalAuthenticationProvider> ExternalAuthenticationProviders { get; set; }

    }

    /// <summary>
    /// 第三方登陆配置
    /// </summary>
    public class ExternalAuthenticationProvider
    {
        /// <summary>
        /// 第三方登陆名称
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// App id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// App secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 在登录页面显示
        /// </summary>
        public bool ShowOnLoginPage { get; set; }
    }
}
