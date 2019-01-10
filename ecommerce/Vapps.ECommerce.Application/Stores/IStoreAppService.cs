using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Stores.Dto;

namespace Vapps.ECommerce.Stores
{

    public interface IStoreAppService
    {
        /// <summary>
        /// 获取所有店铺
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<StoreListDto>> GetStores(GetStoresInput input);

        /// <summary>
        /// 获取所有可用店铺(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto>> GetStoreSelectList();

        /// <summary>
        /// 获取店铺详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetStoreForEditOutput> GetStoreForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateStore(CreateOrUpdateStoreInput input);

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteStore(EntityDto input);
    }
}
