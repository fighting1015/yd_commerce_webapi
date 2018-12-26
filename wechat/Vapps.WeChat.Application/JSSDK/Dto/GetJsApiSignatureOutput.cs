using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.WeChat.Application.JSSDK.Dto
{
    public  class GetJsApiSignatureOutput
    {
        /// <summary>
        /// 公众号 AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
    }
}
