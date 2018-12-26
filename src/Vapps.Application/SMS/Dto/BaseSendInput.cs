using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.SMS.Dto
{
    public class BaseSendInput
    {
        /// <summary>
        /// 验证码结果字符串
        /// </summary>
        public string CaptchaResponse { get; set; }
    }
}
