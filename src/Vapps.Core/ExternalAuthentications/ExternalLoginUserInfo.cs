using Abp.Timing;
using System;
using Vapps.Enums;

namespace Vapps.ExternalAuthentications
{
    [Serializable]
    public class ExternalLoginUserInfo : ExternalAuthUserInfo
    {
        public ExternalLoginUserInfo()
        {
            AccessTokenOutDataTime = Clock.Now;
        }

        /// <summary>
        /// 第三方接口调用凭证
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新凭证
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 调用凭证过期时间
        /// </summary>
        public DateTime AccessTokenOutDataTime { get; set; }
    }

    [Serializable]
    public class ExternalAuthUserInfo
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// 头像url
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// 外部登陆供应商
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 唯一凭证
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// 唯一凭证(全局)
        /// </summary>
        public string UnionProviderKey { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string Surname { get; set; }

        ///// <summary>
        ///// 是否订阅(没有订阅,拉不到其他数据,用户会取消订阅,或者用户只授权但没有关注则不能发送消息)
        ///// </summary>
        //public bool IsSubscribed { get; set; }

        ///// <summary>
        ///// 授权公众号
        ///// 授权后即使用户不关注公众号，也能获取用户的个人信息
        ///// </summary>
        //public bool IsAuthorization { get; set; }
    }
}
