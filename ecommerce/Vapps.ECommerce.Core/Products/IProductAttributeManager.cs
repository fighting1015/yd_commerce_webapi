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
    }
}
