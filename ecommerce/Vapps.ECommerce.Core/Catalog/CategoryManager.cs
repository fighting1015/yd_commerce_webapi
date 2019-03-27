using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Catalog
{
    public class CategoryManager : VappsDomainServiceBase, ICategoryManager
    {
        #region Ctor

        public IRepository<Category, long> CategoryRepository { get; }

        public IQueryable<Category> Categorys => CategoryRepository.GetAll().AsNoTracking();

        public CategoryManager(IRepository<Category, long> categoryRepository)
        {
            this.CategoryRepository = categoryRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据name查找分类
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<Category> FindByNameAsync(string name)
        {
            return await CategoryRepository.FirstOrDefaultAsync(c => c.Name.Contains(name));
        }

        /// <summary>
        /// 根据id查找分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Category> FindByIdAsync(long id)
        {
            return await CategoryRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Category> GetByIdAsync(long id)
        {
            return await CategoryRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="category"></param>
        public virtual async Task CreateAsync(Category category)
        {
            await CategoryRepository.InsertAsync(category);
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="category"></param>
        public virtual async Task UpdateAsync(Category category)
        {
            await CategoryRepository.UpdateAsync(category);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="category"></param>
        public virtual async Task DeleteAsync(Category category)
        {
            await CategoryRepository.DeleteAsync(category);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var category = await CategoryRepository.FirstOrDefaultAsync(id);

            if (category != null)
                await CategoryRepository.DeleteAsync(category);
        }

        #endregion
    }
}
