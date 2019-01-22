using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Products;

namespace Vapps.ECommerce.Products
{

    public interface IProductAttributeManager
    {
        IRepository<ProductAttribute, long> ProductAttributeRepository { get; }

        IQueryable<ProductAttribute> ProductAttributes { get; }

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

        #region ProductAttributeValue

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
