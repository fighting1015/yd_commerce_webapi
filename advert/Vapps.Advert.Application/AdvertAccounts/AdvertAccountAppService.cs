using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Advert.AdvertAccounts.Dto;
using Vapps.Dto;
using Vapps.ECommerce.Products;

namespace Vapps.Advert.AdvertAccounts
{
    public class AdvertAccountAppService : VappsAppServiceBase, IAdvertAccountAppService
    {
        private readonly IProductManager _productManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly ILocalizationManager _localizationManager;

        public AdvertAccountAppService(IProductManager productManager,
            IAdvertAccountManager advertAccountManager,
            ILocalizationManager localizationManager)
        {
            this._productManager = productManager;
            this._advertAccountManager = advertAccountManager;
            this._localizationManager = localizationManager;
        }

        /// <summary>
        /// 获取所有广告账户
        /// </summary>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<AdvertAccountListDto>> GetAccounts(GetAdvertAccountsInput input)
        {
            var query = _advertAccountManager
                .AdvertAccounts
                .WhereIf(!input.ThirdpartyId.IsNullOrWhiteSpace(), r => r.ThirdpartyId.Contains(input.ThirdpartyId))
                .WhereIf(!input.DisplayName.IsNullOrWhiteSpace(), r => r.DisplayName.Contains(input.DisplayName))
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), r => r.UserName.Contains(input.UserName))
                .WhereIf(!input.AdvertChannels.IsNullOrEmpty(), r => input.AdvertChannels.Contains(r.Channel))
                .WhereIf(input.ProductId > 0, r => r.ProductId == input.ProductId);

            var accountCount = await query.CountAsync();

            var accounts = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var accountListDtos = accounts.Select(x =>
            {
                var dto = ObjectMapper.Map<AdvertAccountListDto>(x);
                var product = _productManager.GetByIdAsync(x.ProductId).Result;
                dto.Product = product?.Name ?? string.Empty;
                dto.Channel = x.Channel.GetLocalizedEnum(_localizationManager);
                return dto;
            }).ToList();

            return new PagedResultDto<AdvertAccountListDto>(
                accountCount,
                accountListDtos);
        }

        /// <summary>
        /// 获取所有可用广告账户(下拉框)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<SelectListItemDto<long>>> GetAccountSelectList()
        {
            var query = _advertAccountManager
                .AdvertAccounts;

            var accountCount = await query.CountAsync();
            var accounts = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var advertSelectListItems = accounts.Select(x =>
            {
                return new SelectListItemDto<long>
                {
                    Text = x.DisplayName,
                    Value = x.Id
                };
            }).ToList();
            return advertSelectListItems;
        }

        /// <summary>
        /// 获取广告账户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<GetAdvertAccountForEditOutput> GetAccountForEdit(NullableIdDto<long> input)
        {
            GetAdvertAccountForEditOutput accountDto;

            if (input.Id.HasValue) //Editing existing store?
            {
                var store = await _advertAccountManager.GetByIdAsync(input.Id.Value);
                accountDto = ObjectMapper.Map<GetAdvertAccountForEditOutput>(store);

            }
            else
            {
                accountDto = new GetAdvertAccountForEditOutput();
            }

            return accountDto;

        }

        /// <summary>
        /// 创建或更新广告账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<EntityDto<long>> CreateOrUpdateAccount(CreateOrUpdateAdvertAccountInput input)
        {
            AdvertAccount account;
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                account = await UpdateAccountAsync(input);
            }
            else
            {
                account = await CreateAccountAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = account.Id };
        }

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteAdvertAccount(BatchInput<long> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _advertAccountManager.DeleteAsync(id);
            }
        }

        #region Utilities

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<AdvertAccount> CreateAccountAsync(CreateOrUpdateAdvertAccountInput input)
        {
            var store = ObjectMapper.Map<AdvertAccount>(input);
            await _advertAccountManager.CreateAsync(store);

            return store;
        }

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<AdvertAccount> UpdateAccountAsync(CreateOrUpdateAdvertAccountInput input)
        {
            var store = ObjectMapper.Map<AdvertAccount>(input);
            await _advertAccountManager.UpdateAsync(store);

            return store;
        }

        #endregion
    }
}
