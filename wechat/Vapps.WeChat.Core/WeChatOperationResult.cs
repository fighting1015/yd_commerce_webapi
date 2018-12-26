namespace Vapps.WeChat.Core
{
    public class WeChatOperationResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public string additonData { get; set; }

        /// <summary>
        /// 结果布尔值
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        public void Fail(string errMessage, int errorCode = 0)
        {
            this.Result = false;
            this.ErrorMessage = errMessage;
            this.ErrorCode = errorCode;
        }

        public void Fail(string errMessage, int errorCode, string additonData)
        {
            this.Result = false;
            this.ErrorMessage = errMessage;
            this.ErrorCode = errorCode;
            this.additonData = additonData;
        }

        public void Fail()
        {
            this.Result = false;
        }

        public void Success()
        {
            this.Result = true;
        }

        public void Success(string additonData)
        {
            this.additonData = additonData;
            this.Result = true;
        }
    }
}
