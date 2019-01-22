using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeManager : VappsDomainServiceBase, IProductAttributeManager
    {
        #region Ctor

        public IRepository<ProductAttribute, long> ProductAttributeRepository { get; }

        public IQueryable<ProductAttribute> ProductAttributes => ProductAttributeRepository.GetAll().AsNoTracking();


        public IRepository<ProductAttributeValue, long> ProductAttributeValueRepository { get; }

        public IQueryable<ProductAttributeValue> ProductAttributeValues => ProductAttributeValueRepository.GetAll().AsNoTracking();


        public ProductAttributeManager(IRepository<ProductAttribute, long> attribuyeRepository,
            IRepository<ProductAttributeValue, long> attribuyeValueRepository)
        {
            this.ProductAttributeRepository = attribuyeRepository;
            this.ProductAttributeValueRepository = attribuyeValueRepository;
        }

        #endregion

        #region Attribute

        /// <summary>
        /// 根据名称查找属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttribute> FindByNameAsync(string name)
        {
            return await ProductAttributeRepository.FirstOrDefaultAsync(x => x.Name == name);
        }

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
            if (attribute.Id == 0)
            {
                var entity = await FindByNameAsync(attribute.Name);
                if (entity != null)
                    attribute.Id = entity.Id;
            }

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

        #region Attribute Value

        /// <summary>
        /// 根据名称查找属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeValue> FindValueByNameAsync(string name)
        {
            return await ProductAttributeValueRepository.FirstOrDefaultAsync(x => x.Name == name);
        }

        /// <summary>
        /// 根据id查找属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeValue> FindValueByIdAsync(long id)
        {
            return await ProductAttributeValueRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeValue> GetValueByIdAsync(long id)
        {
            return await ProductAttributeValueRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加/更新属性值
        /// </summary>
        /// <param name="value"></param>
        public virtual async Task CreateOrUpdateValueAsync(ProductAttributeValue value)
        {
            if (value.Id == 0)
            {
                var entity = await FindValueByNameAsync(value.Name);
                if (entity != null)
                    value.Id = entity.Id;
            }

            if (value.Id > 0)
            {
                await UpdateValueAsync(value);
            }
            else
            {
                await CreateValueAsync(value);
            }
        }

        /// <summary>
        /// 添加属性值
        /// </summary>
        /// <param name="attribuye"></param>
        public virtual async Task CreateValueAsync(ProductAttributeValue attribuye)
        {
            await ProductAttributeValueRepository.InsertAsync(attribuye);
        }

        /// <summary>
        /// 更新属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdateValueAsync(ProductAttributeValue attribuye)
        {
            await ProductAttributeValueRepository.UpdateAsync(attribuye);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeleteValueAsync(ProductAttributeValue attribuye)
        {
            await ProductAttributeValueRepository.DeleteAsync(attribuye);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteValueAsync(long id)
        {
            var attribuye = await ProductAttributeValueRepository.FirstOrDefaultAsync(id);

            if (attribuye != null)
                await ProductAttributeValueRepository.DeleteAsync(attribuye);
        }

        #endregion
    }
}
