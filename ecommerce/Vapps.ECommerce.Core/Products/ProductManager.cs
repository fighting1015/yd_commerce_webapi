using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Media;
using Vapps.Media.Cache;

namespace Vapps.ECommerce.Products
{
    public class ProductManager : VappsDomainServiceBase, IProductManager
    {
        #region Ctor

        public IRepository<Product, long> ProductRepository { get; }
        public IQueryable<Product> Products => ProductRepository.GetAll().AsNoTracking();


        public ProductManager(IRepository<Product, long> productRepository)
        {
            this.ProductRepository = productRepository;
        }

        #endregion

        #region Product

        /// <summary>
        /// 根据Sku查找商品
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<Product> FindBySkuAsync(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();

            return await ProductRepository.FirstOrDefaultAsync(p => p.Sku == sku);
        }

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
            var existedProduct = await FindBySkuAsync(product.Sku);
            if (existedProduct != null)
                throw new UserFriendlyException($"Sku : {product.Sku} 已存在");

            await ProductRepository.InsertAsync(product);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="product"></param>
        public virtual async Task UpdateAsync(Product product)
        {
            await ProductRepository.UpdateAsync(product);
        }

        /// <summary>
        /// 更新商品及关联属性
        /// </summary>
        /// <param name="product"></param>
        public virtual async Task UpdateWithRelateAttributeAsync(Product product)
        {
            product.Attributes.Each(a =>
            {
                a.ProductId = product.Id;

                a.Values.Each(v =>
                {
                    v.ProductId = product.Id;
                });
            });

            product.AttributeCombinations.Each(a =>
            {
                a.ProductId = product.Id;
            });

            await ProductRepository.UpdateAsync(product);
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="product"></param>
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
