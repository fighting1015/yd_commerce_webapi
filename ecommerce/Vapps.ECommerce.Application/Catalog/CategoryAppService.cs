using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Catalog.Dto;
using Vapps.Media;

namespace Vapps.ECommerce.Catalog
{
    public class CategoryAppService : VappsAppServiceBase, ICategoryAppService
    {
        private readonly ICategoryManager _catalogyManager;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;

        public CategoryAppService(ICategoryManager catalogyManager,
            ICacheManager cacheManager,
            IPictureManager pictureManager)
        {
            this._catalogyManager = catalogyManager;
            this._cacheManager = cacheManager;
            this._pictureManager = pictureManager;
        }

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<CategoryListDto>> GetCategorys(GetCategoriesInput input)
        {
            var query = _catalogyManager
                .Categorys
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name))
                .WhereIf(input.ParentCategoryId != 0, r => r.ParentCategoryId == input.ParentCategoryId);

            var catalogyCount = await query.CountAsync();

            var catalogys = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var catalogyListDtos = ObjectMapper.Map<List<CategoryListDto>>(catalogys);
            return new PagedResultDto<CategoryListDto>(
                catalogyCount,
                catalogyListDtos);
        }

        /// <summary>
        /// 获取所有可用分类(下拉框)
        /// </summary>
        /// <returns></returns>

        public async Task<List<SelectListItemDto>> GetCategorySelectList()
        {
            var query = _catalogyManager.Categorys;

            var catalogyCount = await query.CountAsync();
            var tempalates = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var catalogySelectListItem = tempalates.Select(x =>
            {
                return new SelectListItemDto
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();
            return catalogySelectListItem;
        }

        /// <summary>
        /// 获取分类详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(NullableIdDto<int> input)
        {
            GetCategoryForEditOutput catalogyDto;

            if (input.Id.HasValue) //Editing existing category?
            {
                var catalogy = await _catalogyManager.GetByIdAsync(input.Id.Value);
                catalogyDto = ObjectMapper.Map<GetCategoryForEditOutput>(catalogy);

                catalogyDto.PictureUrl = await _pictureManager.GetPictureUrlAsync(catalogy.PictureId);
            }
            else
            {
                catalogyDto = new GetCategoryForEditOutput();
            }

            return catalogyDto;
        }

        /// <summary>
        /// 创建或更新分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateCategory(CreateOrUpdateCategoryInput input)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                await UpdateCategoryAsync(input);
            }
            else
            {
                await CreateCategoryAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteCategory(BatchDeleteInput input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _catalogyManager.DeleteAsync(id);
            }
        }

        #region Utilities

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <returns></returns>
        protected virtual async Task CreateCategoryAsync(CreateOrUpdateCategoryInput input)
        {
            var catalogy = ObjectMapper.Map<Category>(input);
            await _catalogyManager.CreateAsync(catalogy);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateCategoryAsync(CreateOrUpdateCategoryInput input)
        {
            var catalogy = ObjectMapper.Map<Category>(input);
            await _catalogyManager.UpdateAsync(catalogy);
        }
    }

    #endregion

}
