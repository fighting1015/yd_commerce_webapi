using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Web.Models.TokenAuth
{
    public class SupplementAuthResultModel
    {
        /// <summary>
        /// 能否登陆
        /// </summary>
        public bool CanLogin { get; set; }

        public int TenantId { get; set; }

        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }
    }
}
