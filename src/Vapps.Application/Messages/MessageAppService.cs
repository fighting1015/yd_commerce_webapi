using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Messages.Dto;

namespace Vapps.Messages
{
    [AbpAuthorize(AdminPermissions.Configuration.HostSettings)]
    public class MessageAppService : VappsAppServiceBase, IMessageAppService
    {
        private readonly IMessageTokenProvider _messageTokenProvider;

        public MessageAppService(IMessageTokenProvider messageTokenProvider)
        {
            this._messageTokenProvider = messageTokenProvider;
        }

        /// <summary>
        /// 获取可用指令
        /// </summary>
        /// <returns></returns>
        public async Task<List<TokensListDto>> GetAvailableTokenList()
        {
            var allowToken = _messageTokenProvider.GetAllowedTokens();
            var result = allowToken.Select(x =>
            {
                return new TokensListDto()
                {
                    Name = L(x.Key),
                    Value = x.Value,
                };
            }).ToList();
            return await Task.FromResult(result);
        }
    }
}
