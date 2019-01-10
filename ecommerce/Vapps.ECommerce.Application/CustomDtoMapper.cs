using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.ECommerce.Stores;
using Vapps.ECommerce.Stores.Dto;
using Vapps.Editions;
using Vapps.Editions.Cache;
using Vapps.MultiTenancy;
using Vapps.Payments;
using Vapps.Payments.Cache;

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

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}