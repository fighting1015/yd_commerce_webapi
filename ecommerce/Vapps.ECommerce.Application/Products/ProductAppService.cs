using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Products.Dto;
using Vapps.Media;

namespace Vapps.ECommerce.Products
{
    public class ProductAppService : VappsAppServiceBase, IProductAppService
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;
        private readonly IProductAttributeManager _productAttributeManager;

        public ProductAppService(IProductManager productManager,
            ICategoryManager categoryManager,
            ICacheManager cacheManager,
            IPictureManager pictureManager,
            IProductAttributeManager productAttributeManager)
        {
            this._productManager = productManager;
            this._categoryManager = categoryManager;
            this._cacheManager = cacheManager;
            this._pictureManager = pictureManager;
            this._productAttributeManager = productAttributeManager;
        }

        /// <summary>
        /// 获取所有商品
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<ProductListDto>> GetProducts(GetProductsInput input)
        {
            var query = _productManager
                .Products
                .Include(p => p.Pictures)
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name));

            var productCount = await query.CountAsync();

            var products = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var productListDtos = products.Select(p =>
            {
                var productDto = ObjectMapper.Map<ProductListDto>(p);

                if (p.Pictures != null && p.Pictures.Any())
                {
                    productDto.PictureUrl = _pictureManager.GetPictureUrl(p.Pictures.FirstOrDefault().PictureId);
                }

                return productDto;
            }).ToList();

            return new PagedResultDto<ProductListDto>(
                productCount,
                productListDtos);
        }

        /// <summary>
        /// 获取所有可用商品(下拉框)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItemDto>> GetProductSelectList()
        {
            var query = _productManager.Products.AsNoTracking();

            var productCount = await query.CountAsync();
            var tempalates = await query
                .OrderByDescending(st => st.Id)
                .ToListAsync();

            var productSelectListItem = tempalates.Select(x =>
            {
                return new SelectListItemDto
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                };
            }).ToList();
            return productSelectListItem;
        }

        /// <summary>
        /// 获取商品的属性及值
        /// </summary>
        /// <returns></returns>
        public async Task<GetProductAttributeMappingOutput> GetProductAttributeMapping(long productId)
        {
            var output = new GetProductAttributeMappingOutput();
            var product = await _productManager.FindByIdAsync(productId);

            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Categories);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Pictures);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Attributes);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.AttributeCombinations);

            PrepareProductAttribute(output.Attributes, product);
            await PrepareProductAttributeCombination(output.AttributeCombinations, product);

            return output;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProductForEditOutput> GetProductForEdit(NullableIdDto<int> input)
        {
            GetProductForEditOutput productDto;

            if (input.Id.HasValue)
            {
                var product = await _productManager.GetByIdAsync(input.Id.Value);

                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Categories);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Pictures);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Attributes);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.AttributeCombinations);

                productDto = ObjectMapper.Map<GetProductForEditOutput>(product);

                productDto.Categories = product.Categories.ToList().Select(i =>
                {
                    var item = new ProductCategoryDto()
                    {
                        Id = i.CategoryId,
                    };
                    item.Name = _categoryManager.GetByIdAsync(item.Id).Result?.Name ?? string.Empty;
                    return item;
                }).ToList();

                PrepareProductAttribute(productDto.Attributes, product);

                await PrepareProductAttributeCombination(productDto.AttributeCombinations, product);

                productDto.Pictures = product.Pictures.OrderBy(p => p.DisplayOrder).ToList().Select(i =>
                  {
                      var item = new ProductPictureDto()
                      {
                          Id = i.PictureId,
                          Url = _pictureManager.GetPictureUrl(i.PictureId)
                      };

                      return item;
                  }).ToList();
            }
            else
            {
                productDto = new GetProductForEditOutput();
            }

            return productDto;
        }

        /// <summary>
        /// 创建或更新商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto<long>> CreateOrUpdateProduct(CreateOrUpdateProductInput input)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                await UpdateProductAsync(input);

                return new EntityDto<long>() { Id = input.Id.Value };
            }
            else
            {
                return await CreateProductAsync(input);
            }
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteProduct(BatchInput<long> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _productManager.DeleteAsync(id);
            }
        }

        #region Utilities

        /// <summary>
        /// 初始化商品属性
        /// </summary>
        /// <param name="attributeDtoList"></param>
        /// <param name="product"></param>
        private void PrepareProductAttribute(List<ProductAttributeDto> attributeDtoList, Product product)
        {
            attributeDtoList = product.Attributes.OrderBy(a => a.DisplayOrder).ToList().Select(attribute =>
              {
                  var item = ObjectMapper.Map<ProductAttributeDto>(attribute);
                  item.Id = attribute.ProductAttributeId;

                  _productAttributeManager.ProductAttributeMappingRepository.EnsurePropertyLoaded(attribute, t => t.ProductAttribute);

                  item.Name = attribute.ProductAttribute.Name;

                  _productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoaded(attribute, t => t.Values);

                  // 属性值
                  item.Values = attribute.Values.OrderBy(v => v.DisplayOrder).ToList().Select(value =>
                    {
                        var valueDto = ObjectMapper.Map<ProductAttributeValueDto>(value);
                        valueDto.Name = value.Name;
                        valueDto.Id = value.PredefinedProductAttributeValueId;
                        valueDto.PictureUrl = _pictureManager.GetPictureUrl(value.PictureId);
                        return valueDto;
                    }).ToList();

                  return item;
              }).ToList();
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<EntityDto<long>> CreateProductAsync(CreateOrUpdateProductInput input)
        {
            var product = ObjectMapper.Map<Product>(input);

            if (input.Categories != null)
            {
                product.Categories = input.Categories.Select(i =>
                {
                    return new ProductCategory()
                    {
                        CategoryId = i.Id,
                    };
                }).ToList();
            }

            if (input.Pictures != null)
            {
                int displayOrder = 0;
                product.Pictures = input.Pictures.Select(i =>
                {
                    return new ProductPicture()
                    {
                        PictureId = i.Id,
                        DisplayOrder = ++displayOrder,
                        ProductId = input.Id ?? 0,
                    };
                }).ToList();
            }

            await CreateOrUpdateProductAttributes(input, product);

            await _productManager.CreateAsync(product);

            await CreateOrUpdateAttributeCombination(input, product);

            await _productManager.UpdateWithRelateAttributeAsync(product);

            return new EntityDto<long>() { Id = product.Id };
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateProductAsync(CreateOrUpdateProductInput input)
        {
            var product = await _productManager.FindByIdAsync(input.Id.Value);

            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Categories);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Pictures);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Attributes);
            await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.AttributeCombinations);

            // 更新基础属性
            product.Name = input.Name;
            product.ShortDescription = input.ShortDescription;
            product.Price = input.Price;
            product.GoodCost = input.GoodCost;
            product.Sku = input.Sku;
            product.ThirdPartySku = input.ThirdPartySku;
            product.StockQuantity = input.StockQuantity;
            product.Height = input.Height;
            product.Weight = input.Weight;
            product.Width = input.Width;
            product.Length = input.Length;
            product.NotifyQuantityBelow = input.NotifyQuantityBelow;
            product.FullDescription = input.FullDescription;

            CreateOrUpdateProductPictures(input, product);

            await CreateOrUpdateProductAttributes(input, product);

            // 执行保存
            await _productManager.UpdateAsync(product);
            await CurrentUnitOfWork.SaveChangesAsync();

            await CreateOrUpdateAttributeCombination(input, product);

            await _productManager.UpdateWithRelateAttributeAsync(product);
        }

        /// <summary>
        /// 创建或更新属性组合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateAttributeCombination(CreateOrUpdateProductInput input, Product product)
        {
            if (input.Id == 0)
            {
                product.AttributeCombinations = new Collection<ProductAttributeCombination>();
            }
            else
            {
                var existItemIds = input.AttributeCombinations.Select(i => i.Id);
                var itemsId2Remove = product.AttributeCombinations.Where(i => !existItemIds.Contains(i.Id)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    product.AttributeCombinations.Remove(item);
                }
            }

            var displayOrder = 0;
            foreach (var combinDto in input.AttributeCombinations)
            {
                // sku去重
                if (combinDto.Sku.IsNullOrWhiteSpace())
                {
                    await IsCombinSkuExisted(input, combinDto);
                }

                ProductAttributeCombination combin = null;
                var attributesJson = JsonConvert.SerializeObject(combinDto.Attributes.GetAttributesJson(product));

                if (input.Id != 0 && combinDto.Id != 0)
                {
                    combin = product.AttributeCombinations.FirstOrDefault(c => c.Id == combinDto.Id
                            || c.AttributesJson == attributesJson);
                }

                if (combin == null)
                    combin = new ProductAttributeCombination();

                combin.AttributesJson = attributesJson;
                combin.Sku = combinDto.Sku;
                combin.ThirdPartySku = combinDto.ThirdPartySku;
                combin.OverriddenPrice = combinDto.OverriddenPrice;
                combin.OverriddenGoodCost = combinDto.OverriddenGoodCost;
                combin.StockQuantity = combinDto.StockQuantity;
                combin.DisplayOrder = displayOrder;

                if (combin.Id == 0)
                    product.AttributeCombinations.Add(combin);
            }
        }

        /// <summary>
        /// Sku 是否已存在
        /// </summary>
        /// <param name="input"></param>
        /// <param name="combinDto"></param>
        /// <returns></returns>
        private async Task IsCombinSkuExisted(CreateOrUpdateProductInput input, AttributeCombinationDto combinDto)
        {
            // sku重复性判断
            var existedCombin = await _productAttributeManager.FindCombinationBySkuAsync(combinDto.Sku);
            if (existedCombin != null && combinDto.Sku == existedCombin.Sku)
            {
                // 重复sku是否在同一个商品中
                if (existedCombin.ProductId != input.Id)
                    throw new UserFriendlyException($"Sku : {combinDto.Sku} 已存在");

                // 已存在的sku是否发生改变
                var existedCombinDto = input.AttributeCombinations
                    .FirstOrDefault(a => a.Id == existedCombin.Id);

                if (existedCombinDto.Sku == existedCombin.Sku)
                    throw new UserFriendlyException($"Sku : {combinDto.Sku} 已存在");
            }
        }

        /// <summary>
        /// 创建或者更新属性
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateProductAttributes(CreateOrUpdateProductInput input, Product product)
        {
            if (product.Id == 0)
            {
                product.Attributes = new Collection<ProductAttributeMapping>();
            }
            else
            {
                var existItemIds = input.Attributes.Select(i => i.Id).ToList();
                var itemsId2Remove = product.Attributes.Where(i => !existItemIds.Contains(i.ProductAttributeId)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    product.Attributes.Remove(item);
                }
            }

            //添加或更新属性
            int displayOrder = 0;
            foreach (var attributeDto in input.Attributes)
            {
                ProductAttributeMapping attributeMapping = null;
                if (product.Id != 0)
                    attributeMapping = product.Attributes.FirstOrDefault(a => a.ProductAttributeId == attributeDto.Id);

                if (attributeMapping == null)
                {
                    // 属性关联
                    attributeMapping = new ProductAttributeMapping()
                    {
                        ProductAttributeId = attributeDto.Id,
                        DisplayOrder = ++displayOrder,
                        Values = new Collection<ProductAttributeValue>()
                    };

                    product.Attributes.Add(attributeMapping);
                }
                else
                {
                    attributeMapping.DisplayOrder = ++displayOrder;
                }

                //添加或更新属性值
                await CreateOrUpdateProductAttributeValues(attributeMapping, attributeDto);
            }
        }

        /// <summary>
        /// 创建更新商品属性值
        /// </summary>
        /// <param name="attributeMapping"></param>
        /// <param name="attributeDto"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateProductAttributeValues(ProductAttributeMapping attributeMapping, ProductAttributeDto attributeDto)
        {
            if (attributeMapping.Id > 0)
            {
                await _productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoadedAsync(attributeMapping, t => t.Values);

                var existItemIds = attributeDto.Values.Select(i => i.Id).ToList();
                var itemsId2Remove = attributeMapping.Values.Where(i => !existItemIds.Contains(i.PredefinedProductAttributeValueId)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    attributeMapping.Values.Remove(item);
                }
            }

            int displayOrder = 0;
            foreach (var valueDto in attributeDto.Values)
            {
                var predefineValue = await _productAttributeManager.GetPredefinedValueByIdAsync(valueDto.Id);

                ProductAttributeValue attributeValue = null;

                if (attributeMapping.Id != 0)
                    attributeValue = attributeMapping.Values
                       .FirstOrDefault(pav => pav.ProductAttributeMappingId > 0
                       && pav.ProductAttributeMapping.ProductAttributeId == attributeDto.Id
                       && pav.PredefinedProductAttributeValueId == valueDto.Id);

                if (attributeValue != null)
                {
                    attributeValue.Name = predefineValue.Name;
                    attributeValue.DisplayOrder = ++displayOrder;
                    attributeValue.PictureId = valueDto.PictureId;
                }
                else
                {
                    attributeValue = new ProductAttributeValue()
                    {
                        Name = predefineValue.Name,
                        DisplayOrder = ++displayOrder,
                        PictureId = valueDto.PictureId,
                        PredefinedProductAttributeValueId = valueDto.Id,
                        ProductId = attributeMapping.ProductId
                    };

                    attributeMapping.Values.Add(attributeValue);
                }
            }
        }

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        private static void CreateOrUpdateProductPictures(CreateOrUpdateProductInput input, Product product)
        {
            //删除不存在的图片
            var existItemIds = input.Pictures.Select(i => i.Id);
            var itemsId2Remove = product.Pictures.Where(i => !existItemIds.Contains(i.PictureId)).ToList();
            foreach (var item in itemsId2Remove)
            {
                product.Pictures.Remove(item);
            }

            //添加或更新图片
            var displayOrder = 0;
            foreach (var itemInput in input.Pictures)
            {
                var item = product.Pictures.FirstOrDefault(x => x.PictureId == itemInput.Id);
                if (item != null)
                {
                    item.DisplayOrder = ++displayOrder;
                }
                else
                {
                    product.Pictures.Add(new ProductPicture()
                    {
                        PictureId = itemInput.Id,
                        DisplayOrder = ++displayOrder
                    });
                }
            }
        }

        /// <summary>
        /// 更新分类
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        private static void CreateOrUpdateCategories(CreateOrUpdateProductInput input, Product product)
        {
            //删除不存在的分类
            var existItemIds = input.Categories.Select(i => i.Id);
            var itemsId2Remove = product.Categories.Where(i => !existItemIds.Contains(i.CategoryId)).ToList();
            foreach (var item in itemsId2Remove)
            {
                product.Categories.Remove(item);
            }

            //添加或更新分类
            foreach (var itemInput in input.Categories)
            {
                var item = product.Categories.FirstOrDefault(x => x.CategoryId == itemInput.Id);
                if (item != null)
                {
                    continue;
                }
                else
                {
                    product.Categories.Add(new ProductCategory()
                    {
                        CategoryId = itemInput.Id,
                    });
                }
            }
        }

        /// <summary>
        /// 初始化属性组合
        /// </summary>
        /// <param name="combinDtoList"></param>
        /// <param name="product"></param>
        private async Task PrepareProductAttributeCombination(List<AttributeCombinationDto> combinDtoList, Product product)
        {
            if (combinDtoList == null)
                combinDtoList = new List<AttributeCombinationDto>();

            var combins = product.AttributeCombinations.OrderBy(c => c.DisplayOrder).ToList();

            foreach (var combination in combins)
            {
                var combinationDto = ObjectMapper.Map<AttributeCombinationDto>(combination);

                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(combination.AttributesJson);

                foreach (var jsonAttribute in jsonAttributeList)
                {
                    var atributeDto = new ProductAttributeDto();

                    var attribute = await _productAttributeManager.GetByIdAsync(jsonAttribute.AttributeId);

                    atributeDto.Id = jsonAttribute.AttributeId;
                    atributeDto.Name = attribute.Name;
                    atributeDto.Values = jsonAttribute.AttributeValues.Select(value =>
                    {
                        var valueDto = new ProductAttributeValueDto();

                        var attributeValue = _productAttributeManager.FindValueById(value.AttributeValueId);
                        if (attributeValue == null)
                            valueDto.Name = _productAttributeManager.GetPredefinedValueById(value.AttributeValueId).Name;
                        else
                            valueDto.Name = attributeValue.Name;

                        valueDto.Id = attributeValue.PredefinedProductAttributeValueId;

                        return valueDto;
                    }).ToList();
                    combinationDto.Attributes.Add(atributeDto);
                }

                combinDtoList.Add(combinationDto);
            }
        }

        #endregion

    }

}
