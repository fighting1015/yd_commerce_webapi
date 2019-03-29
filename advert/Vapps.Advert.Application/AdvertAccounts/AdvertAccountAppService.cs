using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Advert.AdvertAccounts.Dto;
using Vapps.Advert.AdvertAccounts.Sync;
using Vapps.Authorization;
using Vapps.Dto;
using Vapps.ECommerce.Products;

namespace Vapps.Advert.AdvertAccounts
{
    public class AdvertAccountAppService : VappsAppServiceBase, IAdvertAccountAppService
    {
        private readonly IProductManager _productManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IAdvertAccountSyncor _tenantAdvertAccountSyncor;
        private readonly IAdvertAccountSyncor _toutiaoAdvertAccountSyncor;

        public AdvertAccountAppService(IProductManager productManager,
            IAdvertAccountManager advertAccountManager,
            ILocalizationManager localizationManager,
            IIocResolver iocResolver)
        {
            this._productManager = productManager;
            this._advertAccountManager = advertAccountManager;
            this._localizationManager = localizationManager;

            this._tenantAdvertAccountSyncor = iocResolver.Resolve<TenantAdvertAccountSyncor>(typeof(IAdvertAccountSyncor));
            this._toutiaoAdvertAccountSyncor = iocResolver.Resolve<TenantAdvertAccountSyncor>(typeof(IAdvertAccountSyncor));
        }

        /// <summary>
        /// 获取所有广告账户
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Self)]
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
                dto.IsAuthed = x.IsAuth();
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
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Self)]
        public virtual async Task<GetAdvertAccountForEditOutput> GetAccountForEdit(NullableIdDto<long> input)
        {
            GetAdvertAccountForEditOutput accountDto;

            if (input.Id.HasValue) //Editing existing store?
            {
                var account = await _advertAccountManager.GetByIdAsync(input.Id.Value);
                accountDto = ObjectMapper.Map<GetAdvertAccountForEditOutput>(account);
                accountDto.IsAuthed = account.IsAuth();
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
            CheckError(input);

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
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Delete)]
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

        /// <summary>
        /// 获取 授权账户 AccessToken
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Self)]
        public async Task GetAccessToken(GetAccessTokenInput input)
        {
            if (input.AccountId == 0)
            {
                throw new UserFriendlyException("无效账户");
            }

            var account = await _advertAccountManager.GetByIdAsync(input.AccountId);
            if (account == null)
            {
                throw new UserFriendlyException("无效账户");
            }

            if (account.Channel == AdvertChannel.Tencent)
            {
                await _tenantAdvertAccountSyncor.GetAccessTokenAsync(input.Code);
            }
            else
            {
                await _toutiaoAdvertAccountSyncor.GetAccessTokenAsync(input.Code);
            }
        }

        #region Utilities

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Create)]
        protected virtual async Task<AdvertAccount> CreateAccountAsync(CreateOrUpdateAdvertAccountInput input)
        {
            var account = ObjectMapper.Map<AdvertAccount>(input);
            await _advertAccountManager.CreateAsync(account);

            return account;
        }

        private static void CheckError(CreateOrUpdateAdvertAccountInput input)
        {
            if (input.StoreId == 0)
                throw new UserFriendlyException("请选择店铺");
        }

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.AdvertManage.Account.Edit)]
        protected virtual async Task<AdvertAccount> UpdateAccountAsync(CreateOrUpdateAdvertAccountInput input)
        {
            var account = ObjectMapper.Map<AdvertAccount>(input);
            await _advertAccountManager.UpdateAsync(account);

            return account;
        }

        #endregion
    }
}
