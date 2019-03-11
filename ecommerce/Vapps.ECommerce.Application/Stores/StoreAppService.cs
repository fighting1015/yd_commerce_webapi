using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Stores.Dto;
using Vapps.Media;

namespace Vapps.ECommerce.Stores
{
    public class StoreAppService : VappsAppServiceBase, IStoreAppService
    {
        private readonly IStoreManager _storeManager;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;

        public StoreAppService(IStoreManager storeManager,
            ICacheManager cacheManager,
            IPictureManager pictureManager)
        {
            this._storeManager = storeManager;
            this._cacheManager = cacheManager;
            this._pictureManager = pictureManager;
        }


        /// <summary>
        /// 获取所有店铺
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<StoreListDto>> GetStores(GetStoresInput input)
        {
            var query = _storeManager
                .Stores
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name))
                .WhereIf(input.Source != null, r => r.OrderSource == input.Source.Value);

            var storeCount = await query.CountAsync();

            var stores = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var storeListDtos = ObjectMapper.Map<List<StoreListDto>>(stores);
            return new PagedResultDto<StoreListDto>(
                storeCount,
                storeListDtos);
        }

        /// <summary>
        /// 获取所有可用店铺(下拉框)
        /// </summary>
        /// <returns></returns>

        public async Task<List<SelectListItemDto>> GetStoreSelectList()
        {
            var query = _storeManager.Stores;

            var storeCount = await query.CountAsync();
            var tempalates = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var storeSelectListItem = tempalates.Select(x =>
            {
                return new SelectListItemDto
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();
            return storeSelectListItem;
        }

        /// <summary>
        /// 获取店铺详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetStoreForEditOutput> GetStoreForEdit(NullableIdDto<int> input)
        {
            GetStoreForEditOutput storeDto;

            if (input.Id.HasValue) //Editing existing store?
            {
                var store = await _storeManager.GetByIdAsync(input.Id.Value);
                storeDto = ObjectMapper.Map<GetStoreForEditOutput>(store);

                storeDto.PictureUrl = await _pictureManager.GetPictureUrlAsync(store.PictureId);
            }
            else
            {
                storeDto = new GetStoreForEditOutput();
            }

            return storeDto;
        }

        /// <summary>
        /// 创建或更新店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto<long>> CreateOrUpdateStore(CreateOrUpdateStoreInput input)
        {
            Store store;
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                store = await UpdateStoreAsync(input);
            }
            else
            {
                store = await CreateStoreAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = store.Id };
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteStore(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _storeManager.DeleteAsync(id);
            }
        }

        #region Utilities

        /// <summary>
        /// 创建店铺
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<Store> CreateStoreAsync(CreateOrUpdateStoreInput input)
        {
            var store = ObjectMapper.Map<Store>(input);
            await _storeManager.CreateAsync(store);

            return store;
        }

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<Store> UpdateStoreAsync(CreateOrUpdateStoreInput input)
        {
            var store = ObjectMapper.Map<Store>(input);
            await _storeManager.UpdateAsync(store);

            return store;
        }

        #endregion

    }

}
