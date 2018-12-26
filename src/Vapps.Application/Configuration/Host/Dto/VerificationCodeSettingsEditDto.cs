using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Configuration.Host.Dto
{
    public class VerificationCodeSettingsEditDto
    {
        /// <summary>
        /// 启用验证码
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 有效秒数
        /// </summary>
        public int AvailableSecond { get; set; }

        /// <summary>
        /// 验证码最小发送间隔
        /// </summary>
        public int MinimumSendInterval { get; set; }
    }
}
