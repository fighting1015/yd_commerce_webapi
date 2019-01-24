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
        Task<List<ProductAttributeDto>> GetAttributes();

        /// <summary>
        /// 获取所有可用商品属性值
        /// </summary>
        /// <returns></returns>
        Task<List<ProductAttributeValueDto>> GetAttributeValues(long attributeId);
    }
}
