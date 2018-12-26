using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Messages.Dto;

namespace Vapps.Messages
{
    public interface IMessageService
    {
        /// <summary>
        /// 获取可用指令
        /// </summary>
        /// <returns></returns>
        Task<List<TokensListDto>> GetAvailableTokenList();
    }
}
