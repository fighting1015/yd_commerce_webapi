namespace Vapps.SMS.Dto
{
    /// <summary>
    /// 批量发送短信
    /// </summary>
    public class BatchSendSMSInput : BaseSendInput
    {
        /// <summary>
        /// 目标号码数组
        /// </summary>
        public string[] TargetNumbers { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
