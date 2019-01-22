using AutoMapper;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Catalog.Dto;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Products.Dto;
using Vapps.ECommerce.Stores;
using Vapps.ECommerce.Stores.Dto;

namespace Vapps.ECommerce
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //store
            configuration.CreateMap<CreateOrUpdateStoreInput, Store>();
            configuration.CreateMap<Store, CreateOrUpdateStoreInput>();
            configuration.CreateMap<GetStoreForEditOutput, Store>();
            configuration.CreateMap<Store, GetStoreForEditOutput>();
            configuration.CreateMap<Store, StoreListDto>();

            //category
            configuration.CreateMap<CreateOrUpdateCategoryInput, Category>();
            configuration.CreateMap<Category, CreateOrUpdateCategoryInput>();
            configuration.CreateMap<GetCategoryForEditOutput, Category>();
            configuration.CreateMap<Category, GetCategoryForEditOutput>();
            configuration.CreateMap<Category, CategoryListDto>();

            //product
            configuration.CreateMap<CreateOrUpdateProductInput, Product>();
            configuration.CreateMap<ProductCategoryDto, ProductCategory>();
            configuration.CreateMap<ProductCategory, ProductCategoryDto>();
            configuration.CreateMap<Product, CreateOrUpdateProductInput>();
            configuration.CreateMap<Product, ProductListDto>();
            configuration.CreateMap<Product, GetProductForEditOutput>();

            configuration.CreateMap<ProductAttributeDto, ProductAttribute>();
            configuration.CreateMap<ProductAttributeValueDto, ProductAttributeValue>();
            
            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}