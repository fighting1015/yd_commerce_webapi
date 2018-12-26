using Abp.Domain.Entities.Auditing;
using System;

namespace Vapps.WeChat.Core.Users
{
    public class WeChatUser : FullAuditedEntity<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 是否订阅(没有订阅,拉不到其他数据,用户会取消订阅,或者用户只授权但没有关注则不能发送消息)
        /// </summary>
        public bool Subscribe { get; set; }

        /// <summary>
        /// 授权公众号
        /// 授权后即使用户不关注公众号，也能获取用户的个人信息
        /// </summary>
        public bool Authorization { get; set; }

        /// <summary>
        /// 普通用户的标识，对当前开发者帐号唯一
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 公众号Id
        /// </summary>
        public long MpId { get; set; }

        /// <summary>
        /// 普通用户关注公众号的时间
        /// </summary>
        public DateTime? SubscribeTime { get; set; }
    }
}
