using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeManager : VappsDomainServiceBase, IProductAttributeManager
    {
        #region Repository

        public IRepository<ProductAttribute, long> ProductAttributeRepository { get; }
        public IQueryable<ProductAttribute> ProductAttributes => ProductAttributeRepository.GetAll().AsNoTracking();

        public IRepository<ProductAttributeValue, long> ProductAttributeValueRepository { get; }
        public IQueryable<ProductAttributeValue> ProductAttributeValues => ProductAttributeValueRepository.GetAll().AsNoTracking();

        public IRepository<PredefinedProductAttributeValue, long> PredefinedProductAttributeValueRepository { get; }
        public IQueryable<PredefinedProductAttributeValue> PredefinedProductAttributeValues => PredefinedProductAttributeValueRepository.GetAll().AsNoTracking();

        public IRepository<ProductAttributeMapping, long> ProductAttributeMappingRepository { get; }
        public IQueryable<ProductAttributeMapping> ProductAttributeMappings => ProductAttributeMappingRepository.GetAll().AsNoTracking();

        public IRepository<ProductAttributeCombination, long> ProductAttributeCombinationRepository { get; }
        public IQueryable<ProductAttributeCombination> ProductAttributeCombinations => ProductAttributeCombinationRepository.GetAll().AsNoTracking();

        #endregion

        #region Ctor

        public ProductAttributeManager(IRepository<ProductAttribute, long> attributeRepository,
            IRepository<ProductAttributeValue, long> attributeValueRepository,
            IRepository<ProductAttributeMapping, long> attributeMappingRepository,
            IRepository<PredefinedProductAttributeValue, long> predefinedProductAttributeValueRepository,
            IRepository<ProductAttributeCombination, long> productAttributeCombinationRepository)
        {
            this.ProductAttributeRepository = attributeRepository;
            this.ProductAttributeValueRepository = attributeValueRepository;
            this.ProductAttributeMappingRepository = attributeMappingRepository;
            this.PredefinedProductAttributeValueRepository = predefinedProductAttributeValueRepository;
            this.ProductAttributeCombinationRepository = productAttributeCombinationRepository;
        }

        #endregion

        #region Attribute

        /// <summary>
        /// 根据名称查找属性
        /// </summary>
        /// <param name="name"></param>
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
        public virtual ProductAttribute FindById(long id)
        {
            return ProductAttributeRepository.FirstOrDefault(id);
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
        /// <param name="attribute"></param>
        public virtual async Task UpdateAsync(ProductAttribute attribute)
        {
            await ProductAttributeRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="attribute"></param>
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
        /// 根据预设值id查找属性值
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pValueId"></param>
        /// <returns></returns>
        public async Task<ProductAttributeValue> FindValueByPredefinedValueIdAsync(long productId, long pValueId)
        {
            return await ProductAttributeValueRepository
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.PredefinedProductAttributeValueId == pValueId);
        }

        /// <summary>
        /// 根据属性Id和预设值id查找属性值
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="pValueId"></param>
        /// <returns></returns>
        public async Task<ProductAttributeValue> FindValueByAttributeIdAndPredefinedValueIdAsync(long attributeId, long pValueId)
        {
            return await ProductAttributeValueRepository
                .FirstOrDefaultAsync(x => x.ProductAttributeMapping.ProductAttributeId == attributeId
                && x.PredefinedProductAttributeValueId == pValueId);
        }

        /// <summary>
        /// 根据商品id,属性Id和预设值id查找属性值
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributeId"></param>
        /// <param name="pValueId"></param>
        /// <returns></returns>
        public async Task<ProductAttributeValue> FindValueAsync(long productId, long attributeId, long pValueId)
        {
            return await ProductAttributeValueRepository.GetAllIncluding(x => x.ProductAttributeMapping)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.ProductAttributeMapping.ProductAttributeId == attributeId
                && x.PredefinedProductAttributeValueId == pValueId);
        }

        /// <summary>
        /// 根据名称查找属性值
        /// </summary>
        /// <param name="name"></param>
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
        public virtual ProductAttributeValue FindValueById(long id)
        {
            return ProductAttributeValueRepository.FirstOrDefault(id);
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
        public virtual ProductAttributeValue GetValueById(long id)
        {
            return ProductAttributeValueRepository.Get(id);
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
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual async Task UpdateValueAsync(ProductAttributeValue attribute)
        {
            await ProductAttributeValueRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除属性值
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
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
        /// <param name="name"></param>
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
        /// <param name="attributeId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> FindPredefinedValueByNameAsync(long attributeId, string name)
        {
            return await PredefinedProductAttributeValueRepository.FirstOrDefaultAsync(a => a.ProductAttributeId == attributeId && a.Name == name);
        }

        /// <summary>
        /// 根据id获取默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual PredefinedProductAttributeValue GetPredefinedValueById(long id, bool includeDeleted = false)
        {
            if (!includeDeleted)
                return PredefinedProductAttributeValueRepository.Get(id);

            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                return PredefinedProductAttributeValueRepository.Get(id);
            }
        }

        /// <summary>
        /// 根据id获取默认属性值
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<PredefinedProductAttributeValue> GetPredefinedValueByIdAsync(long id, bool includeDeleted = false)
        {
            if (!includeDeleted)
                return await PredefinedProductAttributeValueRepository.GetAsync(id);

            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                return await PredefinedProductAttributeValueRepository.GetAsync(id);
            }
        }

        /// <summary>
        /// 添加/更新默认属性值
        /// </summary>
        /// <param name="value"></param>
        public virtual async Task CreateOrUpdatePredefinedValueAsync(PredefinedProductAttributeValue value)
        {
            if (value.Id == 0)
            {
                var entity = await FindPredefinedValueByNameAsync(value.ProductAttributeId, value.Name);
                if (entity != null)
                {
                    entity.DisplayOrder = value.DisplayOrder;
                    await UpdatePredefinedValueAsync(entity);
                }
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
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual async Task UpdatePredefinedValueAsync(PredefinedProductAttributeValue attribute)
        {
            await PredefinedProductAttributeValueRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除默认属性值
        /// </summary>
        /// <param name="attribute"></param>
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
        /// <param name="productId"></param>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeMapping> FindMappingAsync(long productId, long attributeId)
        {
            return await ProductAttributeMappingRepository.FirstOrDefaultAsync(m => m.ProductId == productId && m.ProductAttributeId == attributeId);
        }

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
        /// 根据商品id获取商品属性
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public virtual async Task<List<ProductAttributeMapping>> GetMappingByProductIdAsync(long productId, bool readOnly = false)
        {
            if (readOnly)
                return await ProductAttributeMappings.Include(pam => pam.Values).Where(pam => pam.ProductId == productId).ToListAsync();
            else
                return await ProductAttributeMappingRepository.GetAll().Include(pam => pam.Values).Where(pam => pam.ProductId == productId).ToListAsync();
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
        /// <param name="attribute"></param>
        public virtual async Task UpdateMappingAsync(ProductAttributeMapping attribute)
        {
            await ProductAttributeMappingRepository.UpdateAsync(attribute);
        }

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="attribute"></param>
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

        #region Attribute combinations

        /// <summary>
        /// 根据Json查找属性组合
        /// </summary>
        /// <param name="attributesJson"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeCombination> FindCombinationByAttributesJsonAsync(string attributesJson)
        {
            if (String.IsNullOrEmpty(attributesJson))
                return null;

            return await ProductAttributeCombinationRepository.FirstOrDefaultAsync(x => x.AttributesJson == attributesJson);
        }

        /// <summary>
        /// 根据Sku查找属性组合
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeCombination> FindCombinationBySkuAsync(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            return await ProductAttributeCombinationRepository.FirstOrDefaultAsync(x => x.Sku == sku);
        }

        /// <summary>
        /// 根据id查找属性组合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeCombination> FindCombinationByIdAsync(long id)
        {
            if (id == 0)
                return null;

            return await ProductAttributeCombinationRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取属性组合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ProductAttributeCombination> GetCombinationByIdAsync(long id)
        {
            return await ProductAttributeCombinationRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加属性组合
        /// </summary>
        /// <param name="combination"></param>
        public virtual async Task CreateCombinationAsync(ProductAttributeCombination combination)
        {
            await ProductAttributeCombinationRepository.InsertAsync(combination);
        }

        /// <summary>
        /// 更新属性组合
        /// </summary>
        /// <param name="combination"></param>
        public virtual async Task UpdateCombinationAsync(ProductAttributeCombination combination)
        {
            await ProductAttributeCombinationRepository.UpdateAsync(combination);
        }

        /// <summary>
        /// 删除属性组合
        /// </summary>
        /// <param name="combination"></param>
        public virtual async Task DeleteCombinationAsync(ProductAttributeCombination combination)
        {
            await ProductAttributeCombinationRepository.DeleteAsync(combination);
        }

        /// <summary>
        /// 删除属性组合
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteCombinationAsync(long id)
        {
            var combination = await ProductAttributeCombinationRepository.FirstOrDefaultAsync(id);

            if (combination != null)
                await ProductAttributeCombinationRepository.DeleteAsync(combination);
        }

        #endregion
    }
}
