using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Products;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeManager : VappsDomainServiceBase, IProductAttributeManager
    {
        #region Ctor

        public IRepository<ProductAttribute, long> ProductAttributeRepository { get; }

        public IQueryable<ProductAttribute> ProductAttributes => ProductAttributeRepository.GetAll().AsNoTracking();

        public ProductAttributeManager(IRepository<ProductAttribute, long> attribuyeRepository)
        {
            this.ProductAttributeRepository = attribuyeRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttribute> FindByIdAsync(long id)
        {
            return await ProductAttributeRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttribute> GetByIdAsync(long id)
        {
            return await ProductAttributeRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加/更新属性
        /// </summary>
        /// <param name="attribute"></param>
        public virtual async Task CreateOrUpdateAsync(ProductAttribute attribute)
        {
            if (attribute.Id > 0)
            {
                await UpdateAsync(attribute);
            }
            else
            {
                await CreateAsync(attribute);
            }
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attribuye"></param>
        public virtual async Task CreateAsync(ProductAttribute attribuye)
        {
            await ProductAttributeRepository.InsertAsync(attribuye);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdateAsync(ProductAttribute attribuye)
        {
            await ProductAttributeRepository.UpdateAsync(attribuye);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeleteAsync(ProductAttribute attribuye)
        {
            await ProductAttributeRepository.DeleteAsync(attribuye);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var attribuye = await ProductAttributeRepository.FirstOrDefaultAsync(id);

            if (attribuye != null)
                await ProductAttributeRepository.DeleteAsync(attribuye);
        }

        #endregion
    }
}
