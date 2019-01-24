using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public IRepository<PredefinedProductAttributeValue, long> PredefinedProductAttributeValueRepository { get; }

        public IQueryable<PredefinedProductAttributeValue> PredefinedProductAttributeValues => PredefinedProductAttributeValueRepository.GetAll().AsNoTracking();

        public IRepository<ProductAttributeMapping, long> ProductAttributeMappingRepository { get; }

        public IQueryable<ProductAttributeMapping> ProductAttributeMappings => ProductAttributeMappingRepository.GetAll().AsNoTracking();


        public ProductAttributeManager(IRepository<ProductAttribute, long> attributeRepository,
            IRepository<ProductAttributeValue, long> attributeValueRepository,
            IRepository<ProductAttributeMapping, long> attributeMappingRepository,
            IRepository<PredefinedProductAttributeValue, long> predefinedProductAttributeValueRepository)
        {
            this.ProductAttributeRepository = attributeRepository;
            this.ProductAttributeValueRepository = attributeValueRepository;
            this.ProductAttributeMappingRepository = attributeMappingRepository;
            this.PredefinedProductAttributeValueRepository = predefinedProductAttributeValueRepository;

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
        [UnitOfWork]
        public virtual async Task CreateOrUpdateAsync(ProductAttribute attribute)
        {
            if (attribute.Id > 0)
            {
                await UpdateAsync(attribute);
            }
            else
            {

                var entity = await FindByNameAsync(attribute.Name);
                if (entity != null)
                {
                    entity.Name = attribute.Name;
                    attribute.Id = entity.Id;
                    await UpdateAsync(entity);
                }
                else
                {
                    await CreateAsync(attribute);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="attribute"></param>
        public virtual async Task CreateAsync(ProductAttribute attribute)
        {
            await ProductAttributeRepository.InsertAsync(attribute);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdateAsync(ProductAttribute attribute)
        {
            await ProductAttributeRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeleteAsync(ProductAttribute attribute)
        {
            await ProductAttributeRepository.DeleteAsync(attribute);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var attribute = await ProductAttributeRepository.FirstOrDefaultAsync(id);

            if (attribute != null)
                await ProductAttributeRepository.DeleteAsync(attribute);
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
        /// <param name="attribute"></param>
        public virtual async Task CreateValueAsync(ProductAttributeValue attribute)
        {
            await ProductAttributeValueRepository.InsertAsync(attribute);
        }

        /// <summary>
        /// 更新属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdateValueAsync(ProductAttributeValue attribute)
        {
            await ProductAttributeValueRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeleteValueAsync(ProductAttributeValue attribute)
        {
            await ProductAttributeValueRepository.DeleteAsync(attribute);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteValueAsync(long id)
        {
            var attribute = await ProductAttributeValueRepository.FirstOrDefaultAsync(id);

            if (attribute != null)
                await ProductAttributeValueRepository.DeleteAsync(attribute);
        }

        #endregion

        #region Predefined Attribute Value

        /// <summary>
        /// 根据名称查找默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> FindPredefinedValueByNameAsync(string name)
        {
            return await PredefinedProductAttributeValueRepository.FirstOrDefaultAsync(x => x.Name == name);
        }

        /// <summary>
        /// 根据id查找默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> FindPredefinedValueByIdAsync(long id)
        {
            return await PredefinedProductAttributeValueRepository.FirstOrDefaultAsync(id);
        }


        /// <summary>
        /// 根据属性id和名称获取默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> FindPredefinedValueByNameAsync(long attributeId, string name)
        {
            return await PredefinedProductAttributeValueRepository.FirstOrDefaultAsync(a => a.ProductAttributeId == attributeId && a.Name == name);
        }

        /// <summary>
        /// 根据id获取默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> GetPredefinedValueByIdAsync(long id)
        {
            return await PredefinedProductAttributeValueRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加/更新默认属性值
        /// </summary>
        /// <param name="value"></param>
        public virtual async Task CreateOrUpdatePredefinedValueAsync(PredefinedProductAttributeValue value)
        {
            if (value.Id == 0)
            {
                var entity = await FindPredefinedValueByNameAsync(value.Name);
                if (entity != null)
                    value.Id = entity.Id;
            }

            if (value.Id > 0)
            {
                await UpdatePredefinedValueAsync(value);
            }
            else
            {
                await CreatePredefinedValueAsync(value);
            }
        }

        /// <summary>
        /// 添加默认属性值
        /// </summary>
        /// <param name="attribute"></param>
        public virtual async Task CreatePredefinedValueAsync(PredefinedProductAttributeValue attribute)
        {
            await PredefinedProductAttributeValueRepository.InsertAsync(attribute);
        }

        /// <summary>
        /// 更新默认属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdatePredefinedValueAsync(PredefinedProductAttributeValue attribute)
        {
            await PredefinedProductAttributeValueRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除默认属性值
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeletePredefinedValueAsync(PredefinedProductAttributeValue attribute)
        {
            await PredefinedProductAttributeValueRepository.DeleteAsync(attribute);
        }

        /// <summary>
        /// 删除默认属性值
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeletePredefinedValueAsync(long id)
        {
            var attribute = await PredefinedProductAttributeValueRepository.FirstOrDefaultAsync(id);

            if (attribute != null)
                await PredefinedProductAttributeValueRepository.DeleteAsync(attribute);
        }

        #endregion

        #region Attribute Mapping

        /// <summary>
        /// 根据id查找商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeMapping> FindMappingByIdAsync(long id)
        {
            return await ProductAttributeMappingRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取商品属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeMapping> GetMappingByIdAsync(long id)
        {
            return await ProductAttributeMappingRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加/更新商品属性
        /// </summary>
        /// <param name="value"></param>
        public virtual async Task CreateOrUpdateMappingAsync(ProductAttributeMapping value)
        {
            if (value.Id > 0)
            {
                await UpdateMappingAsync(value);
            }
            else
            {
                await CreateMappingAsync(value);
            }
        }

        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <param name="attribute"></param>
        public virtual async Task CreateMappingAsync(ProductAttributeMapping attribute)
        {
            await ProductAttributeMappingRepository.InsertAsync(attribute);
        }

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task UpdateMappingAsync(ProductAttributeMapping attribute)
        {
            await ProductAttributeMappingRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="ProductAttribute"></param>
        public virtual async Task DeleteMappingAsync(ProductAttributeMapping attribute)
        {
            await ProductAttributeMappingRepository.DeleteAsync(attribute);
        }

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteMappingAsync(long id)
        {
            var attribute = await ProductAttributeMappingRepository.FirstOrDefaultAsync(id);

            if (attribute != null)
                await ProductAttributeMappingRepository.DeleteAsync(attribute);
        }

        #endregion
    }
}
