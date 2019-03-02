using AutoMapper;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Catalog.Dto;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Orders.Dto;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Products.Dto;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Dto;
using Vapps.ECommerce.Shippings.Dto.Logisticses;
using Vapps.ECommerce.Shippings.Dto.Shipments;
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
            configuration.CreateMap<CreateOrUpdateProductInput, Product>()
                .ForMember(dto => dto.AttributeCombinations, options => options.Ignore());

            configuration.CreateMap<ProductCategoryDto, ProductCategory>();
            configuration.CreateMap<ProductCategory, ProductCategoryDto>();
            configuration.CreateMap<Product, CreateOrUpdateProductInput>();


            configuration.CreateMap<Product, ProductListDto>();
            configuration.CreateMap<Product, GetProductForEditOutput>()
                .ForMember(dto => dto.AttributeCombinations, options => options.Ignore());

            configuration.CreateMap<ProductAttributeDto, ProductAttribute>();
            configuration.CreateMap<ProductAttributeValueDto, ProductAttributeValue>();
            configuration.CreateMap<ProductAttributeValue, ProductAttributeValueDto>()
                .ForMember(dto => dto.PictureUrl, options => options.Ignore());

            configuration.CreateMap<ProductAttribute, ProductAttributeListDto>();


            configuration.CreateMap<ProductAttributeDto, ProductAttributeMapping>();
            //configuration.CreateMap<ProductAttributeMappingDto, ProductAttributeMapping>();

            configuration.CreateMap<ProductAttributeMapping, ProductAttributeDto>();

            configuration.CreateMap<AttributeCombinationDto, ProductAttributeCombination>();
            configuration.CreateMap<ProductAttributeCombination, AttributeCombinationDto>();


            configuration.CreateMap<ProductPictureDto, ProductPicture>();
            configuration.CreateMap<ProductPicture, ProductPictureDto>();

            configuration.CreateMap<OrderDetailDto, Order>();
            configuration.CreateMap<Order, OrderDetailDto>();
            configuration.CreateMap<OrderItem, OrderDetailItemDto>()
                .ForMember(dto => dto.PictureUrl, options => options.Ignore());
            configuration.CreateMap<OrderDetailItemDto, OrderItem>();

            configuration.CreateMap<OrderListDto, Order>();
            configuration.CreateMap<Order, OrderListDto>();
            configuration.CreateMap<OrderListItemDto, OrderItem>();
            configuration.CreateMap<OrderItem, OrderListItemDto>();

            configuration.CreateMap<ShipmentListDto, Shipment>();
            configuration.CreateMap<Shipment, ShipmentListDto>();
            configuration.CreateMap<ShipmentDto, Shipment>();
            configuration.CreateMap<Shipment, ShipmentDto>();
            configuration.CreateMap<ShipmentItemDto, ShipmentItem>();
            configuration.CreateMap<ShipmentItem, ShipmentItemDto>();
            configuration.CreateMap<Shipment, GetShipmentForEditOutput>();


            configuration.CreateMap<LogisticsListDto, Logistics>();
            configuration.CreateMap<Logistics, LogisticsListDto>();
            configuration.CreateMap<CreateOrUpdateLogisticsInput, Logistics>();
            configuration.CreateMap<Logistics, CreateOrUpdateLogisticsInput>();

            configuration.CreateMap<TenantLogistics, TenantLogisticsDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}