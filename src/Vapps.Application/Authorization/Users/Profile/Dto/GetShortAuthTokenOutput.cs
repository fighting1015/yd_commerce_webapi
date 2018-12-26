using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class GetShortAuthTokenOutput
    {
        /// <summary>
        /// 认证短链(可以换取长连接)
        /// </summary>
        public string ShortAuthToken { get; set; }

    }
}
