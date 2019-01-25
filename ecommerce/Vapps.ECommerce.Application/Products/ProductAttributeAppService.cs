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

        #region Attribute

        /// <summary>
        /// 创建或更新属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrUpdateAttributeOutput> CreateOrUpdateAttribute(CreateOrUpdateAttributeInput input)
        {
            var output = new CreateOrUpdateAttributeOutput();
            if (input.Id > 0)
            {
                var attribute = await _productAttributeManager.GetByIdAsync(input.Id.Value);

                attribute.Name = input.Name;
                attribute.DisplayOrder = attribute.DisplayOrder;
                await _productAttributeManager.UpdateAsync(attribute);
                output.Id = attribute.Id;

            }
            else
            {
                var attribute = new ProductAttribute()
                {
                    Name = input.Name,
                    DisplayOrder = input.DisplayOrder,
                };

                await _productAttributeManager.CreateAsync(attribute);
                await CurrentUnitOfWork.SaveChangesAsync();
                output.Id = attribute.Id;
            }

            return output;
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

        #endregion

        #region Attribuete Value

        /// <summary>
        /// 获取所有可用商品属性值
        /// </summary>
        /// <returns></returns>
        public async Task<List<PredefinedProductAttributeValueDto>> GetAttributeValues(long attributeId)
        {
            var query = _productAttributeManager.PredefinedProductAttributeValues.AsNoTracking()
                .Where(a => a.ProductAttributeId == attributeId);

            var attributeValueCount = await query.CountAsync();
            var values = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var productAttributes = values.Select(x =>
            {
                return new PredefinedProductAttributeValueDto
                {
                    Name = x.Name,
                    Id = x.Id
                };
            }).ToList();
            return productAttributes;
        }

        /// <summary>
        /// 创建或更新属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CreateOrUpdateAttributeValueOutput> CreateOrUpdateAttributeValue(CreateOrUpdateAttributeValueInput input)
        {

            var output = new CreateOrUpdateAttributeValueOutput();
            if (input.Id > 0)
            {
                var attributeValue = await _productAttributeManager.GetPredefinedValueByIdAsync(input.Id.Value);

                attributeValue.Name = input.Name;
                attributeValue.DisplayOrder = attributeValue.DisplayOrder;
                await _productAttributeManager.UpdatePredefinedValueAsync(attributeValue);
                output.Id = attributeValue.Id;
            }
            else
            {
                var attributeValue = new PredefinedProductAttributeValue()
                {
                    ProductAttributeId = input.AttributeId,
                    Name = input.Name,
                    DisplayOrder = input.DisplayOrder,
                };

                await _productAttributeManager.CreatePredefinedValueAsync(attributeValue);
                await CurrentUnitOfWork.SaveChangesAsync();
                output.Id = attributeValue.Id;
            }

            return output;
        }


        #endregion
    }
}
