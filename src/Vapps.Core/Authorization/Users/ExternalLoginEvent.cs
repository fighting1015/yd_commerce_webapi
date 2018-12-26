using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ExternalAuthentications;

namespace Vapps.Authorization.Users
{
    /// <summary>
    /// 外部登陆事件
    /// </summary>
    public class ExternalLoginEvent : EventData
    {
        public ExternalLoginEvent(User user, ExternalLoginUserInfo externalLoginInfo)
        {
            this.ExternalLoginInfo = externalLoginInfo;
            this.User = user;
        }

        public User User { get; }

        public ExternalLoginUserInfo ExternalLoginInfo { get; }
    }
}
