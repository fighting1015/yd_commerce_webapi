using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Messages.Dto;

namespace Vapps.Messages
{
    public interface IMessageAppService
    {
        /// <summary>
        /// 获取可用指令
        /// </summary>
        /// <returns></returns>
        Task<List<TokensListDto>> GetAvailableTokenList();
    }
}
