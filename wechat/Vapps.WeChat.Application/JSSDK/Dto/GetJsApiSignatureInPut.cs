using System.ComponentModel.DataAnnotations;

namespace Vapps.WeChat.Application.JSSDK.Dto
{
    public class GetJsApiSignatureInput
    {
        /// <summary>
        /// 当前页面Url
        /// </summary>
        [Required]
        public string SourceUrl { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        [Required]
        public string NonceStr { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [Required]
        public string Timestamp { get; set; }
    }
}
