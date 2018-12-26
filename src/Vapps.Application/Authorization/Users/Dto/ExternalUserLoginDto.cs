using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Authorization.Users.Dto
{
    [AutoMap(typeof(ExternalUserLogin))]
    public class ExternalUserLoginDto
    {
        /// <summary>
        /// 唯一凭证
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 第三方登陆类型
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// OpenId/唯一凭证
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 第三方接口调用凭证
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新凭证
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 调用凭证过期时间
        /// </summary>
        public DateTime AccessTokenOutDataTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
