using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Catalog.Dto;

namespace Vapps.ECommerce.Catalog
{
    public interface ICategoryAppService
    {
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<CategoryListDto>> GetCategorys(GetCategoriesInput input);

        /// <summary>
        /// 获取所有可用分类(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto>> GetCategorySelectList();

        /// <summary>
        /// 获取分类详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCategoryForEditOutput> GetCategoryForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateCategory(CreateOrUpdateCategoryInput input);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteCategory(EntityDto input);
    }
}
