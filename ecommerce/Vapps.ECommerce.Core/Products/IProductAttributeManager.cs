using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{

    public interface IProductAttributeManager
    {
        IRepository<ProductAttribute, long> ProductAttributeRepository { get; }

        IQueryable<ProductAttribute> ProductAttributes { get; }

        IRepository<ProductAttributeValue, long> ProductAttributeValueRepository { get; }

        IQueryable<ProductAttributeValue> ProductAttributeValues { get; }

        IRepository<PredefinedProductAttributeValue, long> PredefinedProductAttributeValueRepository { get; }

        IQueryable<PredefinedProductAttributeValue> PredefinedProductAttributeValues { get; }

        IRepository<ProductAttributeMapping, long> ProductAttributeMappingRepository { get; }

        IQueryable<ProductAttributeMapping> ProductAttributeMappings { get; }

        #region ProductAttribute

        /// <summary>
        /// 根据名称查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttribute> FindByNameAsync(string name);

        /// <summary>
        /// 根据id查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttribute> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttribute> GetByIdAsync(long id);

        /// <summary>
        /// 添加/更新属性
        /// </summary>
        /// <param name="Product"></param>
        Task CreateOrUpdateAsync(ProductAttribute Product);

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="Product"></param>
        Task CreateAsync(ProductAttribute Product);

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="Product"></param>
        Task UpdateAsync(ProductAttribute Product);

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="Product"></param>
        Task DeleteAsync(ProductAttribute Product);

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion

  

        #region Predefined Attribute Value

        /// <summary>
        /// 根据名称查找默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PredefinedProductAttributeValue> FindPredefinedValueByNameAsync(long attributeId, string name);

        /// <summary>
        /// 根据id查找默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PredefinedProductAttributeValue> FindPredefinedValueByIdAsync(long id);

        /// <summary>
        /// 根据id获取默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PredefinedProductAttributeValue> GetPredefinedValueByIdAsync(long id);

        /// <summary>
        /// 添加/更新默认属性值
        /// </summary>
        /// <param name="value"></param>
        Task CreateOrUpdatePredefinedValueAsync(PredefinedProductAttributeValue value);

        /// <summary>
        /// 添加默认属性值
        /// </summary>
        /// <param name="attribute"></param>
        Task CreatePredefinedValueAsync(PredefinedProductAttributeValue attribute);

        /// <summary>
        /// 更新默认属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        Task UpdatePredefinedValueAsync(PredefinedProductAttributeValue attribute);

        /// <summary>
        /// 删除默认属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        Task DeletePredefinedValueAsync(PredefinedProductAttributeValue attribute);

        /// <summary>
        /// 删除默认属性值
        /// </summary>
        /// <param name="id"></param>
        Task DeletePredefinedValueAsync(long id);

        #endregion

        #region ProductAttribute Mapping

        /// <summary>
        /// 根据id查找商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeMapping> FindMappingByIdAsync(long id);

        /// <summary>
        /// 根据id获取商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeMapping> GetMappingByIdAsync(long id);

        /// <summary>
        /// 添加/更新商品属性
        /// </summary>
        /// <param name="Product"></param>
        Task CreateOrUpdateMappingAsync(ProductAttributeMapping Product);

        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="Product"></param>
        Task CreateMappingAsync(ProductAttributeMapping Product);

        /// <summary>
        /// 修改商品属性
        /// </summary>
        /// <param name="Product"></param>
        Task UpdateMappingAsync(ProductAttributeMapping Product);

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="Product"></param>
        Task DeleteMappingAsync(ProductAttributeMapping Product);

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="id"></param>
        Task DeleteMappingAsync(long id);

        #endregion

        #region ProductAttributeValue

        /// <summary>
        /// 根据属性Id和预设值id查找属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeValue> FindValueByAttributeIdAndPredefinedValueIdAsync(long attributeId,long pValueId);

        /// <summary>
        /// 根据名称查找属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeValue> FindValueByNameAsync(string name);

        /// <summary>
        /// 根据id查找属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeValue> FindValueByIdAsync(long id);

        /// <summary>
        /// 根据id获取属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductAttributeValue> GetValueByIdAsync(long id);

        /// <summary>
        /// 添加/更新属性值
        /// </summary>
        /// <param name="Product"></param>
        Task CreateOrUpdateValueAsync(ProductAttributeValue Product);

        /// <summary>
        /// 添加属性值
        /// </summary>
        /// <param name="Product"></param>
        Task CreateValueAsync(ProductAttributeValue Product);

        /// <summary>
        /// 修改属性值
        /// </summary>
        /// <param name="Product"></param>
        Task UpdateValueAsync(ProductAttributeValue Product);

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="Product"></param>
        Task DeleteValueAsync(ProductAttributeValue Product);

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="id"></param>
        Task DeleteValueAsync(long id);

        #endregion
    }
}
