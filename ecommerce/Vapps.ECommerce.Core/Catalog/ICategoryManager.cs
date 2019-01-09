using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Core.Catalog
{
    public interface ICategoryManager
    {
        IRepository<Category, int> CategoryRepository { get; }

        IQueryable<Category> Categorys { get; }

        #region Category

        /// <summary>
        /// 根据id查找商品分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> FindByIdAsync(int id);

        /// <summary>
        /// 根据id获取商品分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Category> GetByIdAsync(int id);

        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="category"></param>
        Task CreateAsync(Category category);

        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="category"></param>
        Task UpdateAsync(Category category);

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="category"></param>
        Task DeleteAsync(Category category);

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        #endregion
    }
}