using Abp.Domain.Entities.Auditing;
using System;

namespace Vapps.WeChat.Core.WeChatMps
{
    public class WeChatMp : FullAuditedEntity<long>
    {
        /// <summary>
        /// 授权方AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 已授权
        /// </summary>
        public bool Authorized { get; set; }

        /// <summary>
        /// 公众号昵称
        /// </summary>
        public string MpNickName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 微信公众号原始 Id
        /// </summary>
        public string OriginalId { get; set; }

        /// <summary>
        /// 授权方公众号所设置的微信号，可能为空
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 是否开通微信门店功能
        /// </summary>
        public bool OpenStore { get; set; }

        /// <summary>
        /// 是否开通微信扫商品功能
        /// </summary>
        public bool OpenScan { get; set; }

        /// <summary>
        /// 是否开通微信支付功能
        /// </summary>
        public bool OpenPay { get; set; }

        /// <summary>
        /// 是否开通微信卡券功能
        /// </summary>
        public bool OpenCard { get; set; }

        /// <summary>
        /// 是否开通微信摇一摇功能
        /// </summary>
        public bool OpenShake { set; get; }

        /// <summary>
        /// 二维码图片的URL，开发者最好自行也进行保存
        /// </summary>
        public string QrcodeUrl { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        public string AuthorizationInfo { get; set; }

        /// <summary>
        /// 公众号的接口调用凭据
        /// </summary>
        public string AuthorizerAccessToken { get; set; }

        /// <summary>
        /// 接口调用凭据刷新令牌
        /// </summary>
        public string AuthorizerRefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? AccessTokenExpriesInUtc { get; set; }

        /// <summary>
        /// 公众号js api ticket
        /// </summary>
        public string JsApiTicket { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? JsApiTicketExpriesInUtc { get; set; }

        /// <summary>
        /// 公众号card api ticket
        /// </summary>
        public string CardApiTicket { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? CardApiTicketExpriesInUtc { get; set; }

        /// <summary>
        /// 是否是默认公众号
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 公众号类型
        /// </summary>
        public ServiceTypeInfo Type { get; set; }

        /// <summary>
        /// 认证类型
        /// </summary>
        public VerifyType VerifyType { get; set; }
    }
}
