using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Dto;
using Vapps.ECommerce.Shippings.Dto;
using Vapps.ECommerce.Shippings.Dto.Logisticses;
using Vapps.Helpers;

namespace Vapps.ECommerce.Shippings
{
    public class LogisticsAppService : VappsAppServiceBase, ILogisticsAppService
    {
        private readonly ILogisticsManager _logisticsManager;
        private readonly ICacheManager _cacheManager; // TODO: 待实现

        public LogisticsAppService(ILogisticsManager logisticsManager,
            ICacheManager cacheManager)
        {
            this._logisticsManager = logisticsManager;
            this._cacheManager = cacheManager;
        }

        #region Logistics

        /// <summary>
        /// 获取所有物流（平台）
        /// </summary>
        /// <returns></returns>
        
        public async Task<PagedResultDto<LogisticsListDto>> GetLogisticses(GetLogisticsesInput input)
        {
            var query = _logisticsManager
             .Logisticses.OrderBy(l => l.Name);

            var logisticsCount = await query.CountAsync();

            var logisticses = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var logisticsListDtos = logisticses.Select(l => PrepareLogisticsListDto(l)).OrderBy(l => l.Prefix).ToList();
            return new PagedResultDto<LogisticsListDto>(
                logisticsCount,
                logisticsListDtos);
        }

        /// <summary>
        /// 获取所有可用物流(下拉框)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItemDto<long>>> GetLogisticsSelectList()
        {
            var query = _logisticsManager.Logisticses;

            var storeCount = await query.CountAsync();
            var _logisticses = await query
                .OrderBy(st => st.Memo)
                .ToListAsync();

            var storeSelectListItem = _logisticses.Select(x =>
            {
                return new SelectListItemDto<long>
                {
                    Text = x.Name,
                    Value = x.Id
                };
            }).ToList();
            return storeSelectListItem;
        }

        /// <summary>
        /// 获取物流详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Logistics.Self)]
        public async Task<GetLogisticsForEditOutput> GetLogisticsForEdit(NullableIdDto<int> input)
        {
            GetLogisticsForEditOutput logisticsDto;

            if (input.Id.HasValue)
            {
                var logistics = await _logisticsManager.GetByIdAsync(input.Id.Value);
                logisticsDto = ObjectMapper.Map<GetLogisticsForEditOutput>(logistics);
            }
            else
            {
                logisticsDto = new GetLogisticsForEditOutput();
            }

            return logisticsDto;
        }

        /// <summary>
        /// 添加或更新物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Logistics.Self)]
        public async Task<EntityDto<long>> CreateOrUpdateLogistics(CreateOrUpdateLogisticsInput input)
        {
            Logistics logistics = null;
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                logistics = await UpdateLogisticsAsync(input);
            }
            else
            {
                logistics = await CreateLogisticsAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = logistics.Id };
        }

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteLogistics(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _logisticsManager.DeleteAsync(id);
            }
        }

        #endregion

        #region Tenant Logistics

        /// <summary>
        /// 获取所有租户自选物流
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.TenantLogistics.Self)]
        public async Task<PagedResultDto<TenantLogisticsDto>> GetTenantLogisticses(GetLogisticsesInput input)
        {
            var query = _logisticsManager
             .TenantLogisticses.OrderBy(l => l.DisplayOrder);

            var logisticsCount = await query.CountAsync();

            var logisticses = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var logisticsListDtos = ObjectMapper.Map<List<TenantLogisticsDto>>(logisticses);
            return new PagedResultDto<TenantLogisticsDto>(
                logisticsCount,
                logisticsListDtos);
        }

        /// <summary>
        /// 获取所有可用自选物流(下拉框)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItemDto<long>>> GetTenantLogisticsSelectList()
        {
            var query = _logisticsManager.TenantLogisticses;

            var storeCount = await query.CountAsync();
            var _logisticses = await query
                .OrderBy(st => st.DisplayOrder)
                .ToListAsync();

            var storeSelectListItem = _logisticses.Select(x =>
            {
                return new SelectListItemDto<long>
                {
                    Text = x.Name,
                    Value = x.Id
                };
            }).ToList();
            return storeSelectListItem;
        }

        /// <summary>
        /// 获取物流详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.TenantLogistics.Self)]
        public async Task<GetLogisticsForEditOutput> GetTenantLogisticsForEdit(NullableIdDto<int> input)
        {
            GetLogisticsForEditOutput logisticsDto;

            if (input.Id.HasValue)
            {
                var tenantLogistics = await _logisticsManager.GetTenantLogisticsByIdAsync(input.Id.Value);
                logisticsDto = ObjectMapper.Map<GetLogisticsForEditOutput>(tenantLogistics);

                var logistics = await _logisticsManager.GetByIdAsync(tenantLogistics.LogisticsId);
                logisticsDto.Key = logistics.Key;
                logisticsDto.Memo = logistics.Memo;
            }
            else
            {
                logisticsDto = new GetLogisticsForEditOutput();
            }

            return logisticsDto;
        }

        /// <summary>
        /// 添加或更新自选物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.TenantLogistics.Self)]
        public async Task<EntityDto<long>> CreateOrUpdateTenantLogistics(CreateOrUpdateTenantLogisticsInput input)
        {
            TenantLogistics tenantLogistics = null;
            if (input.Id > 0)
            {
                tenantLogistics = await UpdateTenantLogisticsAsync(input);
            }
            else
            {
                tenantLogistics = await CreateTenantLogisticsAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = tenantLogistics.Id };
        }

        /// <summary>
        /// 删除租户物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteTenantLogistics(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _logisticsManager.DeleteTenantLogisticsAsync(id);
            }
        }


        #endregion

        #region Utilities

        private LogisticsListDto PrepareLogisticsListDto(Logistics logistics)
        {
            var dto = ObjectMapper.Map<LogisticsListDto>(logistics);

            dto.Prefix = CommonHelper.ChangeByName(dto.Name.Substring(0, 1));

            if (dto.Prefix.IsNullOrWhiteSpace())
                dto.Prefix = logistics.Memo?.Substring(0, 1) ?? string.Empty;

            return dto;
        }

        /// <summary>
        /// 添加物流
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Logistics.Create)]
        protected virtual async Task<Logistics> CreateLogisticsAsync(CreateOrUpdateLogisticsInput input)
        {
            var logistics = await _logisticsManager.FindByNameAsync(input.Name);

            if (logistics != null)
            {
                ObjectMapper.Map(input, logistics);
                await _logisticsManager.UpdateAsync(logistics);
            }
            else
            {
                logistics = ObjectMapper.Map<Logistics>(input);
                await _logisticsManager.CreateAsync(logistics);
            }
            return logistics;
        }

        /// <summary>
        /// 更新物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Logistics.Edit)]
        protected virtual async Task<Logistics> UpdateLogisticsAsync(CreateOrUpdateLogisticsInput input)
        {
            var logistics = await _logisticsManager.FindByIdAsync(input.Id.Value);

            ObjectMapper.Map(input, logistics);

            await _logisticsManager.UpdateAsync(logistics);

            return logistics;
        }

        /// <summary>
        /// 添加租户物流
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.TenantLogistics.Create)]
        protected virtual async Task<TenantLogistics> CreateTenantLogisticsAsync(CreateOrUpdateTenantLogisticsInput input)
        {
            TenantLogistics tenantLogistic = await _logisticsManager.FindTenantLogisticsByLogisticsIdAsync(input.LogisticsId);

            var logistic = await _logisticsManager.FindByIdAsync(input.LogisticsId);
            if (tenantLogistic != null)
            {
                tenantLogistic.LogisticsId = logistic.Id;
                tenantLogistic.Name = logistic.Name;
                tenantLogistic.DisplayOrder = input.DisplayOrder;
                await _logisticsManager.UpdateTenantLogisticsAsync(tenantLogistic);
            }
            else
            {
                tenantLogistic = new TenantLogistics();
                tenantLogistic.LogisticsId = logistic.Id;
                tenantLogistic.Name = logistic.Name;
                tenantLogistic.DisplayOrder = input.DisplayOrder;
                await _logisticsManager.CreateTenantLogisticsAsync(tenantLogistic);
            }

            return tenantLogistic;
        }

        /// <summary>
        /// 更新租户物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.TenantLogistics.Edit)]
        protected virtual async Task<TenantLogistics> UpdateTenantLogisticsAsync(CreateOrUpdateTenantLogisticsInput input)
        {
            var tenantLogistic = await _logisticsManager.FindTenantLogisticsByIdAsync(input.Id);

            tenantLogistic.DisplayOrder = input.DisplayOrder;

            await _logisticsManager.UpdateTenantLogisticsAsync(tenantLogistic);

            return tenantLogistic;
        }

        #endregion
    }
}
