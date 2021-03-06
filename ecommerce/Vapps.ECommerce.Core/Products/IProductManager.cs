﻿using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public interface IProductManager
    {
        IRepository<Product, long> ProductRepository { get; }

        IQueryable<Product> Products { get; }

        #region Product

        /// <summary>
        /// 根据Sku查找商品
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        Task<Product> FindBySkuAsync(string sku);


        /// <summary>
        /// 根据id查找商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetByIdAsync(long id);

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="product"></param>
        Task CreateAsync(Product product);

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="product"></param>
        Task UpdateAsync(Product product);

        /// <summary>
        /// 更新商品及关联属性
        /// </summary>
        /// <param name="product"></param>
        Task UpdateWithRelateAttributeAsync(Product product);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="product"></param>
        Task DeleteAsync(Product product);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
