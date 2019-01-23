using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public class ProductManager : VappsDomainServiceBase, IProductManager
    {
        #region Ctor

        public IRepository<Product, long> ProductRepository { get; }

        public IQueryable<Product> Products => ProductRepository.GetAll().AsNoTracking();

        private readonly IProductAttributeManager _productAttributeManager;


        public ProductManager(IRepository<Product, long> productRepository,
            IProductAttributeManager productAttributeManager)
        {
            this.ProductRepository = productRepository;
            this._productAttributeManager = productAttributeManager;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Product> FindByIdAsync(long id)
        {
            return await ProductRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Product> GetByIdAsync(long id)
        {
            return await ProductRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="product"></param>
        public virtual async Task CreateAsync(Product product)
        {
            // 创建或更新属性
            foreach (var attribute in product.Attributes)
            {
                //if (attribute.ProductAttributeId == 0)
                //{
                //    _productAttributeManager.FindByNameAsync(attribute.na);

                    //_productAttributeManager.CreateAsync();
                //}
            }


            // 创建或更新属性
            foreach (var combination in product.AttributeCombinations)
            {

            }

            // 创建或更新属性组合


            await ProductRepository.InsertAsync(product);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="Product"></param>
        public virtual async Task UpdateAsync(Product product)
        {
            await ProductRepository.UpdateAsync(product);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="Product"></param>
        public virtual async Task DeleteAsync(Product product)
        {
            await ProductRepository.DeleteAsync(product);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var product = await ProductRepository.FirstOrDefaultAsync(id);

            if (product != null)
                await ProductRepository.DeleteAsync(product);
        }

        #endregion
    }
}
