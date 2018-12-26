namespace Vapps.SMS
{
    /// <summary>
    /// 短信发送结果
    /// </summary>
    public class SendResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 成功个数
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// 失败个数
        /// </summary>
        public int FailCount { get; set; }
    }
}
