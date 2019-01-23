using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Products.Dto;
using Vapps.Media;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Abp.Domain.Repositories;
using Vapps.ECommerce.Catalog;
using Newtonsoft.Json;
using Abp.Domain.Uow;

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
            var query = _productManager.Products;

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

                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Categorys);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Pictures);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Attributes);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.AttributeCombinations);

                productDto = ObjectMapper.Map<GetProductForEditOutput>(product);

                productDto.Categorys = product.Categorys.Select(i =>
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

        private void PrepareProductAttribute(GetProductForEditOutput productDto, Product product)
        {
            productDto.Attributes = product.Attributes.Select(attribute =>
            {
                var item = ObjectMapper.Map<ProductAttributeDto>(attribute);
                item.Name = _productAttributeManager.GetByIdAsync(attribute.ProductAttributeId).Result?.Name ?? string.Empty;

                _productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoaded(attribute, t => t.Values);

                item.Values = attribute.Values.Select(value =>
                {
                    var valueDto = ObjectMapper.Map<ProductAttributeValueDto>(value);
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

            product.Categorys = input.Categorys.Select(i =>
            {
                var item = ObjectMapper.Map<ProductCategory>(i);
                return item;
            }).ToList();

            await CreateOrUpdateAttribute(input, product);

            product.Pictures = input.Pictures.Select(i =>
            {
                return ObjectMapper.Map<ProductPicture>(i);
            }).ToList();

            await _productManager.CreateAsync(product);
            await CreateOrUpdateAttributeCombinations(input, product);

            await _productManager.UpdateAsync(product);
        }

        private async Task CreateOrUpdateAttributeCombinations(CreateOrUpdateProductInput input, Product product)
        {
            product.AttributeCombinations = new Collection<ProductAttributeCombination>();
            foreach (var combinDto in input.AttributeCombinations)
            {
                var combin = ObjectMapper.Map<ProductAttributeCombination>(combinDto);
                var jsonAttributes = await CreateOrUpdateJsonAttribute(combinDto);

                combin.AttributesJson = JsonConvert.SerializeObject(jsonAttributes);
                product.AttributeCombinations.Add(combin);
            }
        }

        private async Task CreateOrUpdateAttribute(CreateOrUpdateProductInput input, Product product)
        {
            if (product.Id == 0)
                product.Attributes = new Collection<ProductAttributeMapping>();

            var existItemIds = input.Attributes.Select(i => i.Id);
            var itemsId2Remove = product.Attributes.Where(i => !existItemIds.Contains(i.Id)).ToList();

            //删除不存在的属性
            foreach (var item in itemsId2Remove)
            {
                item.IsDeleted = true;
                product.Attributes.Remove(item);
            }

            //添加或更新属性
            foreach (var attribueDto in input.Attributes)
            {
                var attribue = ObjectMapper.Map<ProductAttribute>(attribueDto);

                // 创建不存在的属性
                if (attribue.Id == 0)
                    await _productAttributeManager.CreateOrUpdateAsync(attribue);

                attribueDto.Id = attribue.Id;

                var attributeMapping = new ProductAttributeMapping()
                {
                    ProductAttributeId = attribue.Id,
                    DisplayOrder = attribueDto.DisplayOrder,
                    Values = attribueDto.Values.Select(i =>
                    {
                        var value = ObjectMapper.Map<ProductAttributeValue>(i);
                        value.Name = i.Name;
                        return value;
                    }).ToList()
                };

                product.Attributes.Add(attributeMapping);
            }
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task UpdateProductAsync(CreateOrUpdateProductInput input)
        {
            var product = await _productManager.FindByIdAsync(input.Id.Value);

            UpdateProductPictures(input, product);
            UpdateProductAttribute(input, product);

            await _productManager.UpdateAsync(product);
        }

        #region Utility

        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="input"></param>
        /// <param name="product"></param>
        private static void UpdateProductPictures(CreateOrUpdateProductInput input, Product product)
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


        private async Task<List<JsonProductAttribute>> CreateOrUpdateJsonAttribute(AttributeCombinationDto combinDto)
        {
            var jsonAttributes = new List<JsonProductAttribute>();

            foreach (var attributeDto in combinDto.Attributes)
            {
                var jsonAttributeItem = new JsonProductAttribute();

                if (attributeDto.Id == 0)
                {
                    var productAttribute = await _productAttributeManager.FindByNameAsync(attributeDto.Name);
                    jsonAttributeItem.AttributeId = productAttribute.Id;
                }
                else
                {
                    jsonAttributeItem.AttributeId = attributeDto.Id;
                }

                jsonAttributeItem.AttributeName = attributeDto.Name;

                foreach (var value in attributeDto.Values)
                {
                    var jsonAttributeValue = new JsonProductAttributeValue();

                    if (value.Id == 0)
                    {
                        var productAttribute = await _productAttributeManager.FindValueByNameAsync(value.Name);
                        jsonAttributeValue.AttributeValueId = productAttribute.Id;
                        jsonAttributeValue.AttributeValueName = productAttribute.Name;
                    }
                    else
                    {
                        jsonAttributeValue.AttributeValueId = attributeDto.Id;
                        jsonAttributeValue.AttributeValueName = attributeDto.Name;
                    }
                    jsonAttributeValue.DisplayOrder = value.DisplayOrder;
                    jsonAttributeItem.AttributeValues.Add(jsonAttributeValue);
                }

                jsonAttributes.Add(jsonAttributeItem);
            }

            return jsonAttributes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDto"></param>
        /// <param name="product"></param>
        private void PrepareProductAttributeCombination(GetProductForEditOutput productDto, Product product)
        {
            productDto.AttributeCombinations = new List<AttributeCombinationDto>();
            foreach (var combination in product.AttributeCombinations)
            {
                var combinationDto = ObjectMapper.Map<AttributeCombinationDto>(combination);

                var atributeDtoList = new List<ProductAttributeDto>();

                var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(combination.AttributesJson);

                foreach (var jsonAttribute in jsonAttributeList)
                {
                    var atributeDto = new ProductAttributeDto();
                    atributeDto.Id = jsonAttribute.AttributeId;
                    atributeDto.Name = jsonAttribute.AttributeName;
                    atributeDto.Values = jsonAttribute.AttributeValues.Select(value =>
                    {
                        var valueDto = new ProductAttributeValueDto();
                        valueDto.Id = value.AttributeValueId;
                        valueDto.AttributeId = jsonAttribute.AttributeId;
                        valueDto.Name = value.AttributeValueName;
                        valueDto.DisplayOrder = value.DisplayOrder;
                        return valueDto;
                    }).ToList();
                    combinationDto.Attributes.Add(atributeDto);
                }

                productDto.AttributeCombinations.Add(combinationDto); ;
            }
        }


        #endregion
    }

    #endregion
}
