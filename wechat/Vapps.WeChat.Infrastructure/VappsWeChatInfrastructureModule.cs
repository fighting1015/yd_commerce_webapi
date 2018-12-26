using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Cache.Redis;
using Senparc.CO2NET.Threads;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using Vapps.Configuration;
using Abp.Extensions;

namespace Vapps.WeChat
{
    [DependsOn(
      typeof(VappsCoreModule)
    )]
    public class VappsWeChatInfrastructureModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VappsWeChatInfrastructureModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }

        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsWeChatInfrastructureModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            /* 微信配置开始
             * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
            */
            //RegisterWeixinCache();      //注册分布式缓存（按需，如果需要，必须放在第一个）
            RegisterWeixinThreads();    //激活微信缓存及队列线程（必须）
            RegisterSenparcWeixin();    //注册所用微信公众号的账号信息（按需）
            //RegisterWeixinThirdParty(); //注册微信第三方平台（按需）
            /* 微信配置结束 */
        }

        /// <summary>
        /// 自定义缓存策略
        /// </summary>
        private void RegisterWeixinCache()
        {
            //如果留空，默认为localhost（默认端口）
            #region  Redis配置

            var redisConfiguration = _appConfiguration[ConfigKeys.Redis];
            RedisManager.ConfigurationOption = redisConfiguration;

            //如果不执行下面的注册过程，则默认使用本地缓存
            if (!string.IsNullOrEmpty(redisConfiguration) && redisConfiguration != "")
            {
                CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//Redis
            }

            #endregion

            #region Memcached 配置

            //var memcached = _appConfiguration["Abp:Memcached"];
            //if (string.IsNullOrEmpty(memcached))
            //{
            //    var servers = JsonToDictionary(memcached);
            //    MemcachedObjectCacheStrategy.RegisterServerList(servers);
            //}

            #endregion
        }

        /// <summary>
        /// 激活微信缓存
        /// </summary>
        private void RegisterWeixinThreads()
        {
            ThreadUtility.Register();
        }

        /// <summary>
        /// 注册所用微信公众号的账号信息
        /// </summary>
        private void RegisterSenparcWeixin()
        {
            var wechatCommonHepler = IocManager.Resolve<WeChatCommonHepler>();

            var loginProviderSetting = wechatCommonHepler.GetLoginProviderSetting(WeChatConsts.MPNAME);

            //注册公众号
            if(!loginProviderSetting.AppId.IsNullOrWhiteSpace())
            {
                AccessTokenContainer.Register(loginProviderSetting.AppId, loginProviderSetting.AppSecret);
            }
            //注册小程序
            //AccessTokenContainer.Register(_appConfiguration["WxOpenAppId"], _appConfiguration["WxOpenAppSecret"]);
        }

        /// <summary>
        /// 注册微信第三方平台(若有)
        /// </summary>
        private void RegisterWeixinThirdParty()
        {

        }

        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        private Dictionary<string, int> JsonToDictionary(string jsonData)
        {
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
