using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
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
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(input.Name));

            var productCount = await query.CountAsync();

            var products = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var productListDtos = ObjectMapper.Map<List<ProductListDto>>(products);
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

                productDto.Categories = product.Categories.Select(i =>
                {
                    var item = ObjectMapper.Map<ProductCategoryDto>(i);
                    item.Name = _categoryManager.GetByIdAsync(item.CategoryId).Result?.Name ?? string.Empty;
                    return item;
                }).ToList();

                PrepareProductAttribute(productDto, product);

                PrepareProductAttributeCombination(productDto, product);

                productDto.Pictures = product.Pictures.Select(i =>
                {
                    var item = ObjectMapper.Map<ProductPictureDto>(i);
                    item.PictureUrl = _pictureManager.GetPictureUrl(i.Id);
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
        public async Task CreateOrUpdateProduct(CreateOrUpdateProductInput input)
        {
            if (input.Id.HasValue && input.Id.Value > 0)
            {
                await UpdateProductAsync(input);
            }
            else
            {
                await CreateProductAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteProduct(BatchDeleteInput<long> input)
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
        /// <param name="productDto"></param>
        /// <param name="product"></param>
        private void PrepareProductAttribute(GetProductForEditOutput productDto, Product product)
        {
            productDto.Attributes = product.Attributes.Select(attribute =>
            {
                var item = ObjectMapper.Map<ProductAttributeDto>(attribute);
                item.Id = attribute.ProductAttributeId;

                _productAttributeManager.ProductAttributeMappingRepository.EnsurePropertyLoaded(attribute, t => t.ProductAttribute);

                item.Name = attribute.ProductAttribute.Name;

                _productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoaded(attribute, t => t.Values);

                // 属性值
                item.Values = attribute.Values.Select(value =>
                {
                    var valueDto = ObjectMapper.Map<PredefinedProductAttributeValueDto>(value);

                    valueDto.Id = value.PredefinedProductAttributeValueId;

                    return valueDto;
                }).ToList();

                return item;
            }).ToList();
        }

        /// <summary>
        /// 创建商品
        /// </summary>
        /// <returns></returns>
        protected virtual async Task CreateProductAsync(CreateOrUpdateProductInput input)
        {
            var product = ObjectMapper.Map<Product>(input);

            if (input.Categories != null)
            {
                product.Categories = input.Categories.Select(i =>
                {
                    var item = ObjectMapper.Map<ProductCategory>(i);
                    return item;
                }).ToList();
            }

            if (input.Pictures != null)
            {
                product.Pictures = input.Pictures.Select(i =>
                {
                    return ObjectMapper.Map<ProductPicture>(i);
                }).ToList();
            }

            // 创建属性
            await CreateOrUpdateProductAttributes(input, product);

            // 创建属性组合
            CreateOrUpdateAttributeCombination(input, product);

            await _productManager.CreateAsync(product);
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateProductAsync(CreateOrUpdateProductInput input)
        {
            var product = await _productManager.FindByIdAsync(input.Id.Value);

            CreateOrUpdateProductPictures(input, product);

            await CreateOrUpdateProductAttributes(input, product);

            CreateOrUpdateAttributeCombination(input, product);

            await _productManager.UpdateAsync(product);
        }

        /// <summary>
        /// 创建或更新属性组合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        private void CreateOrUpdateAttributeCombination(CreateOrUpdateProductInput input, Product product)
        {
            if (product.Id == 0)
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

            foreach (var combinDto in input.AttributeCombinations)
            {
                ProductAttributeCombination combin = null;
                var attributesJson = JsonConvert.SerializeObject(combinDto.Attributes.GetAttributesJson());

                if (product.Id != 0)
                    combin = product.AttributeCombinations.FirstOrDefault(c => c.Id == combinDto.Id
                            || c.AttributesJson == attributesJson);

                if (combin == null)
                    combin = new ProductAttributeCombination();

                combin.AttributesJson = attributesJson;
                combin.Sku = combinDto.Sku;
                combin.ThirdPartySku = combinDto.ThirdPartySku;
                combin.OverriddenPrice = combinDto.OverriddenPrice;
                combin.OverriddenGoodCost = combinDto.OverriddenGoodCost;
                combin.StockQuantity = combinDto.StockQuantity;

                if (combin.Id == 0)
                    product.AttributeCombinations.Add(combin);
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
                var existItemIds = input.Attributes.Select(i => i.Id);
                var itemsId2Remove = product.Attributes.Where(i => !existItemIds.Contains(i.Id)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    product.Attributes.Remove(item);
                }
            }

            //添加或更新属性
            foreach (var attributeDto in input.Attributes)
            {
                ProductAttributeMapping attributeMapping = null;
                if (product.Id != 0)
                    attributeMapping = product.Attributes.FirstOrDefault(a => a.Id == attributeDto.Id);

                if (attributeMapping == null)
                {
                    // 属性关联
                    attributeMapping = new ProductAttributeMapping()
                    {
                        ProductAttributeId = attributeDto.Id,
                        DisplayOrder = attributeDto.DisplayOrder,
                        Values = new Collection<ProductAttributeValue>()
                    };

                    product.Attributes.Add(attributeMapping);
                }
                else
                {
                    attributeMapping.DisplayOrder = attributeDto.DisplayOrder;
                }

                //添加或更新属性值
                await CreateOrUpdateProductAttributeValues(attributeMapping, attributeDto);
            }
        }

        /// <summary>
        /// 属性商品属性
        /// </summary>
        /// <param name="attributeMapping"></param>
        /// <param name="attributeDto"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateProductAttributeValues(ProductAttributeMapping attributeMapping, ProductAttributeMappingDto attributeDto)
        {
            foreach (var valueDto in attributeDto.Values)
            {
                var predefineValue = await _productAttributeManager.GetPredefinedValueByIdAsync(valueDto.Id);
                //var attributeValue = await _productAttributeManager.FindValueByAttributeIdAndPredefinedValueIdAsync(attributeDto.Id, valueDto.Id);

                ProductAttributeValue attributeValue = null;

                if (attributeMapping.Id != 0)
                    attributeValue = attributeMapping.Values
                       .FirstOrDefault(pav => pav.ProductAttributeMapping.ProductAttributeId == attributeDto.Id
                       && pav.PredefinedProductAttributeValueId == valueDto.Id);

                if (attributeValue != null)
                {
                    attributeValue.Name = predefineValue.Name;
                    attributeValue.DisplayOrder = valueDto.DisplayOrder;
                    attributeValue.PictureId = valueDto.PictureId;
                }
                else
                {
                    attributeValue = new ProductAttributeValue()
                    {
                        Name = predefineValue.Name,
                        DisplayOrder = valueDto.DisplayOrder,
                        PictureId = valueDto.PictureId,
                        PredefinedProductAttributeValueId = valueDto.Id
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
            var itemsId2Remove = product.Pictures.Where(i => !existItemIds.Contains(i.Id)).ToList();

            //删除不存在的图片
            foreach (var item in itemsId2Remove)
            {
                product.Pictures.Remove(item);
            }

            //添加或更新图片
            foreach (var itemInput in input.Pictures)
            {
                if (itemInput.Id.HasValue && itemInput.Id.Value > 0)
                {
                    var item = product.Pictures.FirstOrDefault(x => x.Id == itemInput.Id.Value);
                    if (item != null)
                    {
                        item.PictureId = itemInput.PictureId;
                        item.DisplayOrder = itemInput.DisplayOrder;
                    }
                }
                else
                {
                    product.Pictures.Add(new ProductPicture()
                    {
                        PictureId = itemInput.PictureId,
                        DisplayOrder = itemInput.DisplayOrder,
                    });
                }
            }
        }

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        private static void UpdateProductAttribute(CreateOrUpdateProductInput input, Product product)
        {
            var existItemIds = input.Attributes.Select(i => i.Id);
            var itemsId2Remove = product.Attributes.Where(i => !existItemIds.Contains(i.Id)).ToList();

            //删除不存在的属性
            foreach (var item in itemsId2Remove)
            {
                item.IsDeleted = true;
                product.Attributes.Remove(item);
            }

            //添加或更新属性
            foreach (var itemInput in input.Attributes)
            {
                if (itemInput.Id > 0)
                {
                    var item = product.Attributes.FirstOrDefault(x => x.Id == itemInput.Id);
                    if (item != null)
                    {
                        item.ProductAttributeId = itemInput.Id;
                        item.DisplayOrder = itemInput.DisplayOrder;
                    }
                }
                else
                {
                    product.Attributes.Add(new ProductAttributeMapping()
                    {

                    });
                }
            }
        }

        /// <summary>
        /// 初始化属性组合
        /// </summary>
        /// <param name="productDto"></param>
        /// <param name="product"></param>
        private void PrepareProductAttributeCombination(GetProductForEditOutput productDto, Product product)
        {
            productDto.AttributeCombinations = new List<AttributeCombinationDto>();
            foreach (var combination in product.AttributeCombinations)
            {
                var combinationDto = ObjectMapper.Map<AttributeCombinationDto>(combination);

                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(combination.AttributesJson);

                foreach (var jsonAttribute in jsonAttributeList)
                {
                    var atributeDto = new ProductAttributeMappingDto();
                    atributeDto.Id = jsonAttribute.AttributeId;
                    atributeDto.Values = jsonAttribute.AttributeValues.Select(value =>
                    {
                        var valueDto = new ProductAttributeValueDto();
                        valueDto.Id = value.AttributeValueId;
                        valueDto.DisplayOrder = value.DisplayOrder;
                        return valueDto;
                    }).ToList();
                    combinationDto.Attributes.Add(atributeDto);
                }

                productDto.AttributeCombinations.Add(combinationDto);
            }
        }

        #endregion

    }

}
