using AutoMapper;
using Vapps.ECommerce.Catalog;
using Vapps.ECommerce.Catalog.Dto;
using Vapps.ECommerce.Customers;
using Vapps.ECommerce.Customers.Dto;
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

            configuration.CreateMap<OrderListDto, Order>();
            configuration.CreateMap<Order, OrderListDto>();
            configuration.CreateMap<OrderDetailDto, Order>();
            configuration.CreateMap<Order, OrderDetailDto>();
            configuration.CreateMap<OrderItem, OrderDetailItemDto>()
                .ForMember(dto => dto.PictureUrl, options => options.Ignore());
            configuration.CreateMap<OrderDetailItemDto, OrderItem>();

            configuration.CreateMap<OrderListItemDto, OrderItem>();
            configuration.CreateMap<OrderItem, OrderListItemDto>();
            configuration.CreateMap<OrderItemDto, OrderItem>();

            configuration.CreateMap<CreateOrUpdateOrderInput, Order>()
                .ForMember(dto => dto.DiscountAmount, options => options.Ignore())
                .ForMember(dto => dto.RefundedAmount, options => options.Ignore())
                .ForMember(dto => dto.ShippingAmount, options => options.Ignore())
                .ForMember(dto => dto.RewardAmount, options => options.Ignore())
                .ForMember(dto => dto.SubtotalAmount, options => options.Ignore())
                .ForMember(dto => dto.SubTotalDiscountAmount, options => options.Ignore())
                .ForMember(dto => dto.TotalAmount, options => options.Ignore())
                .ForMember(dto => dto.PaymentMethodAdditionalFee, options => options.Ignore())
                .ForMember(dto => dto.OrderStatus, options => options.Ignore())
                .ForMember(dto => dto.PaymentStatus, options => options.Ignore())
                .ForMember(dto => dto.ShippingStatus, options => options.Ignore())
                .ForMember(dto => dto.OrderType, options => options.Ignore())
                .ForMember(dto => dto.PaymentType, options => options.Ignore())
                .ForMember(dto => dto.OrderSource, options => options.Ignore())
                .ForMember(dto => dto.Items, options => options.Ignore());

            configuration.CreateMap<OrderItemDto, OrderItem>()
               .ForMember(dto => dto.Price, options => options.Ignore())
               .ForMember(dto => dto.UnitPrice, options => options.Ignore())
               .ForMember(dto => dto.DiscountAmount, options => options.Ignore());

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

            configuration.CreateMap<Customer, CustomerListDto>();
            configuration.CreateMap<Customer, CustomerDetailDto>();
            configuration.CreateMap<CreateOrUpdateCustomerInput, Customer>();
            configuration.CreateMap<Customer, CreateOrUpdateCustomerInput>();
            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}