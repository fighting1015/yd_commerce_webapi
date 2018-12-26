using Abp.AutoMapper;
using Senparc.Weixin.TenPay.V3;
using System;
using System.Collections.Generic;

namespace Vapps.WeChat.Payments.Jobs
{
    [Serializable]
    [AutoMapFrom(typeof(OrderQueryResult))]
    public class ProcessWeChatPaymentJobArgs
    {
        public string return_code { get; set; }
        public string return_msg { get; set; }
        //
        // 摘要:
        //     微信分配的公众账号ID
        public string appid { get; set; }
        //
        // 摘要:
        //     微信支付分配的商户号
        public string mch_id { get; set; }
        //
        // 摘要:
        //     子商户公众账号ID
        public string sub_appid { get; set; }
        //
        // 摘要:
        //     子商户号
        public string sub_mch_id { get; set; }
        //
        // 摘要:
        //     随机字符串，不长于32 位
        public string nonce_str { get; set; }
        //
        // 摘要:
        //     签名
        public string sign { get; set; }
        //
        // 摘要:
        //     SUCCESS/FAIL
        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        //
        // 摘要:
        //     附加数据，原样返回
        public string attach { get; set; }
        //
        // 摘要:
        //     商户系统的订单号，与请求一致。
        public string out_trade_no { get; set; }
        //
        // 摘要:
        //     微信支付订单号
        public string transaction_id { get; set; }
        //
        // 摘要:
        //     单个代金券支付金额, $n为下标，从0开始编号 coupon_fee_$n
        public IList<int> coupon_fee_values { get; set; }
        //
        // 摘要:
        //     代金券ID, $n为下标，从0开始编号 coupon_id_$n
        public IList<string> coupon_id_values { get; set; }
        //
        // 摘要:
        //     CASH--充值代金券 NO_CASH---非充值代金券 订单使用代金券时有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_$0
        //     coupon_type_$n
        public IList<string> coupon_type_values { get; set; }
        //
        // 摘要:
        //     代金券使用数量
        public string coupon_count { get; set; }
        public string coupon_fee { get; set; }
        //
        // 摘要:
        //     货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        public string cash_fee_type { get; set; }
        //
        // 摘要:
        //     现金支付金额订单现金支付金额
        public string cash_fee { get; set; }
        //
        // 摘要:
        //     订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010
        public string time_end { get; set; }
        //
        // 摘要:
        //     货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY
        public string fee_type { get; set; }
        //
        // 摘要:
        //     订单总金额，单位为分
        public string total_fee { get; set; }
        //
        // 摘要:
        //     商品详情[服务商]
        public string detail { get; set; }
        //
        // 摘要:
        //     银行类型，采用字符串类型的银行标识
        public string bank_type { get; set; }
        //
        // 摘要:
        //     SUCCESS—支付成功 REFUND—转入退款 NOTPAY—未支付 CLOSED—已关闭 REVOKED—已撤销（刷卡支付） USERPAYING--用户支付中
        //     PAYERROR--支付失败(其他原因，如银行返回失败)
        public string trade_state { get; set; }
        //
        // 摘要:
        //     调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，MICROPAY
        public string trade_type { get; set; }
        //
        // 摘要:
        //     是否关注子公众账号[服务商]
        public string sub_is_subscribe { get; set; }
        //
        // 摘要:
        //     用户子标识[服务商]
        public string sub_openid { get; set; }
        //
        // 摘要:
        //     用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        public string is_subscribe { get; set; }
        //
        // 摘要:
        //     用户在商户appid下的唯一标识
        public string openid { get; set; }
        //
        // 摘要:
        //     微信支付分配的终端设备号
        public string device_info { get; set; }
        public string settlement_total_fee { get; set; }
        //
        // 摘要:
        //     对当前查询订单状态的描述和下一步操作的指引
        public string trade_state_desc { get; set; }
    }
}
