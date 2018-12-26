using System;
using Vapps.ExternalAuthentications;

namespace Vapps.Authorization.Accounts.Job
{
    [Serializable]
    public class CreateOrUpdateAccountJobArgs
    {
        public long UserId { get; set; }

        public ExternalLoginUserInfo ExternalLoginInfo { get; set; }
    }
}
