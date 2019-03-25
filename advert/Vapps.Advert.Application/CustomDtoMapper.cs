using AutoMapper;
using Vapps.Advert.AdvertAccounts;
using Vapps.Advert.AdvertAccounts.Dto;
using Vapps.Advert.AdvertStatistics;
using Vapps.Advert.AdvertStatistics.Dto;

namespace Vapps.Advert
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {

            configuration.CreateMap<AdvertAccount, AdvertAccountListDto>()
                .ForMember(dto => dto.Channel, options => options.Ignore());
            configuration.CreateMap<AdvertAccount, GetAdvertAccountForEditOutput>();
            configuration.CreateMap<CreateOrUpdateAdvertAccountInput, AdvertAccount>();
            configuration.CreateMap<AdvertAccount, CreateOrUpdateAdvertAccountInput>();

            configuration.CreateMap<AdvertDailyStatistic, DailyStatisticDto>()
                .ForMember(dto => dto.Items, options => options.Ignore());
            configuration.CreateMap<DailyStatisticDto, AdvertDailyStatistic>()
                .ForMember(dto => dto.Items, options => options.Ignore());

            configuration.CreateMap<DailyStatisticItemDto, AdvertDailyStatisticItem>();
            configuration.CreateMap<AdvertDailyStatisticItem, DailyStatisticItemDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}