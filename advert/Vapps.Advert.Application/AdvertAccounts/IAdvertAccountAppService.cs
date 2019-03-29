using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Advert.AdvertAccounts.Dto;
using Vapps.Dto;

namespace Vapps.Advert.AdvertAccounts
{
    public interface IAdvertAccountAppService
    {
        /// <summary>
        /// 获取所有广告账户
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<AdvertAccountListDto>> GetAccounts(GetAdvertAccountsInput input);

        /// <summary>
        /// 获取所有可用广告账户(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto<long>>> GetAccountSelectList();

        /// <summary>
        /// 获取广告账户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetAdvertAccountForEditOutput> GetAccountForEdit(NullableIdDto<long> input);

        /// <summary>
        /// 创建或更新广告账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateAccount(CreateOrUpdateAdvertAccountInput input);

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteAdvertAccount(BatchInput<long> input);

        /// <summary>
        /// 获取 授权账户 AccessToken
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task GetAccessToken(GetAccessTokenInput input);
    }
}
