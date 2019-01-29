using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Products.Dto;

namespace Vapps.ECommerce.Products
{
    public interface IProductAttributeAppService
    {
        /// <summary>
        /// 获取所有可用商品属性
        /// </summary>
        /// <returns></returns>
        Task<List<ProductAttributeListDto>> GetAttributes();

        /// <summary>
        /// 创建或更新属性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrUpdateAttributeOutput> CreateOrUpdateAttribute(CreateOrUpdateAttributeInput input);

        /// <summary>
        /// 获取所有可用商品属性值
        /// </summary>
        /// <returns></returns>
        Task<List<PredefinedProductAttributeValueDto>> GetAttributeValues(long attributeId);

        /// <summary>
        /// 创建或更新属性值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CreateOrUpdateAttributeValueOutput> CreateOrUpdateAttributeValue(CreateOrUpdateAttributeValueInput input);
    }
}
