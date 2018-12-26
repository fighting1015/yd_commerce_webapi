using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Reflection.Extensions;

namespace Vapps.Localization
{
    public static class VappsLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    VappsConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(VappsLocalizationConfigurer).GetAssembly(),
                        "Vapps.Localization.Vapps"
                    )
                )
            );

            localizationConfiguration.Sources.Add(
               new DictionaryBasedLocalizationSource(
               VappsConsts.AdminLocalizationSourceName,
               new JsonEmbeddedFileLocalizationDictionaryProvider(
                    typeof(VappsLocalizationConfigurer).GetAssembly(),
                       "Vapps.Localization.Admin"
               )
             )
           );

            localizationConfiguration.Sources.Add(
               new DictionaryBasedLocalizationSource(
               VappsConsts.ServerSideLocalizationSourceName,
               new JsonEmbeddedFileLocalizationDictionaryProvider(
                    typeof(VappsLocalizationConfigurer).GetAssembly(),
                       "Vapps.Localization.ServerSide"
               )
             )
           );

            localizationConfiguration.Sources.Add(
                 new DictionaryBasedLocalizationSource(
                 VappsConsts.BusinessLocalizationSourceName,
                 new JsonEmbeddedFileLocalizationDictionaryProvider(
                      typeof(VappsLocalizationConfigurer).GetAssembly(),
                         "Vapps.Localization.BusinessCenter"
                 )
               )
             );

            localizationConfiguration.Sources.Add(
                 new DictionaryBasedLocalizationSource(
                 VappsConsts.UserLocalizationSourceName,
                 new JsonEmbeddedFileLocalizationDictionaryProvider(
                      typeof(VappsLocalizationConfigurer).GetAssembly(),
                         "Vapps.Localization.UserCenter"
                 )
               )
             );
        }
    }
}