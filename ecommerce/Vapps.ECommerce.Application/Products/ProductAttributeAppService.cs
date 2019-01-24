using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Products.Dto;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeAppService : VappsAppServiceBase, IProductAttributeAppService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IProductAttributeManager _productAttributeManager;

        public ProductAttributeAppService(IProductAttributeManager productAttributeManager,
            ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._productAttributeManager = productAttributeManager;
        }

        /// <summary>
        /// 获取所有可用商品属性
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductAttributeDto>> GetAttributes()
        {
            var query = _productAttributeManager.ProductAttributes.AsNoTracking();

            var attributeCount = await query.CountAsync();
            var tempalates = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var productSelectListItem = tempalates.Select(x =>
            {
                return new ProductAttributeDto
                {
                    Name = x.Name,
                    Id = x.Id
                };
            }).ToList();
            return productSelectListItem;
        }

        /// <summary>
        /// 获取所有可用商品属性值
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductAttributeValueDto>> GetAttributeValues(long attributeId)
        {
            var query = _productAttributeManager.PredefinedProductAttributeValues.AsNoTracking()
                .Where(a => a.ProductAttributeId == attributeId);

            var attributeValueCount = await query.CountAsync();
            var values = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var productAttributes = values.Select(x =>
            {
                return new ProductAttributeValueDto
                {
                    Name = x.Name,
                    Id = x.Id
                };
            }).ToList();
            return productAttributes;
        }
    }
}
