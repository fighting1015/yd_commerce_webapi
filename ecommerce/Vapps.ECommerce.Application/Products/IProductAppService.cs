using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Products.Dto;

namespace Vapps.ECommerce.Products
{
    public interface IProductAppService
    {
        /// <summary>
        /// 获取所有店铺
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<ProductListDto>> GetProducts(GetProductsInput input);

        /// <summary>
        /// 获取所有可用店铺(下拉框)
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItemDto>> GetProductSelectList();

        /// <summary>
        /// 获取店铺详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetProductForEditOutput> GetProductForEdit(NullableIdDto<int> input);

        /// <summary>
        /// 创建或更新店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateProduct(CreateOrUpdateProductInput input);

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteProduct(BatchDeleteInput<long> input);
    }
}
