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

namespace Vapps.ECommerce.Products
{
    public class ProductAppService : VappsAppServiceBase, IProductAppService
    {
        private readonly IProductManager _productManager;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;
        private readonly IProductAttributeManager _productAttributeManager;

        public ProductAppService(IProductManager productManager,
            ICacheManager cacheManager,
            IPictureManager pictureManager,
            IProductAttributeManager productAttributeManager)
        {
            this._productManager = productManager;
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

            if (input.Id.HasValue) //Editing existing product?
            {
                var product = await _productManager.GetByIdAsync(input.Id.Value);
                productDto = ObjectMapper.Map<GetProductForEditOutput>(product);


                //productDto.PictureUrl = await _pictureManager.GetPictureUrlAsync(product.PictureId);
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

            product.Attributes = new Collection<ProductAttributeMapping>();
            foreach (var attribueDto in input.Attributes)
            {
                var attribue = ObjectMapper.Map<ProductAttribute>(attribueDto);

                await _productAttributeManager.CreateOrUpdateAsync(attribue);
                attribueDto.Id = attribue.Id;

                var attributeMapping = new ProductAttributeMapping()
                {
                    ProductAttributeId = attribue.Id,
                    DisplayOrder = attribueDto.DisplayOrder,
                };

                attributeMapping.Values = attribueDto.Values.Select(i =>
                {
                    return ObjectMapper.Map<ProductAttributeValue>(i);
                }).ToList();

                product.Attributes.Add(attributeMapping);
            }

            product.Pictures = input.Pictures.Select(i =>
            {
                return ObjectMapper.Map<ProductPicture>(i);
            }).ToList();

            product.AttributeCombinations = input.AttributeCombinations.Select(i =>
            {
                var combin = ObjectMapper.Map<ProductAttributeCombination>(i);

                return combin;
            }).ToList();

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

        #endregion
    }

    #endregion
}
