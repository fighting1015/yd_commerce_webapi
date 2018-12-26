using Abp.Application.Features;
using Abp.Localization;
using Abp.Runtime.Validation;
using Abp.UI.Inputs;

namespace Vapps.Features
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(
                AppFeatures.MaxUserCount,
                defaultValue: "0", //0 = unlimited
                displayName: L("MaximumUserCount"),
                description: L("MaximumUserCount_Description"),
                inputType: new SingleLineStringInputType(new NumericValueValidator(0, int.MaxValue))
            )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
            {
                ValueTextNormalizer = value => new FixedLocalizableString(value),
                IsVisibleOnPricingTable = false
            };

            context.Create(
               AppFeatures.MaxPictureCount,
               defaultValue: "1",
               displayName: L("Features.MaxPictureCount"),
               description: L("Features.MaxPictureCount.Description"),
               inputType: new SingleLineStringInputType(new NumericValueValidator(1, int.MaxValue))
           )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
           {
               ValueTextNormalizer = value => new FixedLocalizableString(value),
               IsVisibleOnPricingTable = true
           };

            context.Create(
                AppFeatures.RemoveAdvert,
                defaultValue: "false",
                displayName: L("Features.RemoveAdvert"),
                description: L("Features.RemoveAdvert.Description"),
                inputType: new CheckboxInputType()
            )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
            {
                TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                IsVisibleOnPricingTable = true
            };

            context.Create(
                AppFeatures.RemoveLogo,
                defaultValue: "false",
                displayName: L("Features.RemoveLogo"),
                description: L("Features.RemoveLogo.Description"),
                inputType: new CheckboxInputType()
                )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
                {
                    TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                    IsVisibleOnPricingTable = true
                };

            context.Create(
               AppFeatures.AddressMap,
               defaultValue: "false",
               displayName: L("Features.AddressMap"),
               description: L("Features.AddressMap.Description"),
               inputType: new CheckboxInputType()
               )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
               {
                   TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                   IsVisibleOnPricingTable = true
               };

            CreateNotificationFeature(context);

            CreateDataStatisticsFeature(context);
        }

        private static void CreateDataStatisticsFeature(IFeatureDefinitionContext context)
        {
            var dataStatisticsFeature = context.Create(
                                        AppFeatures.DataStatistics,
                                        defaultValue: "false",
                                        displayName: L("Features.DataStatistics"),
                                        description: L("Features.DataStatistics.Description"),
                                        inputType: new CheckboxInputType());

            dataStatisticsFeature[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
            {
                TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                IsVisibleOnPricingTable = true
            };

            dataStatisticsFeature.CreateChildFeature(
                AppFeatures.AccessRegionsData,
                defaultValue: "false",
                displayName: L("Features.AccessRegionsData"),
                description: L("Features.AccessRegionsData.Description"),
               inputType: new CheckboxInputType()
               )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
               {
                   TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                   IsVisibleOnPricingTable = true
               };

            dataStatisticsFeature.CreateChildFeature(
               AppFeatures.AccessSourcesData,
               defaultValue: "false",
               displayName: L("Features.AccessSourcesData"),
               description: L("Features.AccessSourcesData.Description"),
               inputType: new CheckboxInputType()
               )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
               {
                   TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                   IsVisibleOnPricingTable = true
               };

            dataStatisticsFeature.CreateChildFeature(
                AppFeatures.AccessTimesData,
                defaultValue: "false",
                displayName: L("Features.AccessTimesData"),
                description: L("Features.AccessTimesData.Description"),
                inputType: new CheckboxInputType()
                )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
                {
                    TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                    IsVisibleOnPricingTable = true
                };

            dataStatisticsFeature.CreateChildFeature(
                AppFeatures.ConverRatesData,
                defaultValue: "false",
                displayName: L("Features.ConverRatesData"),
                description: L("Features.ConverRatesData.Description"),
                inputType: new CheckboxInputType()
                )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
                {
                    TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                    IsVisibleOnPricingTable = true
                };
        }

        private static void CreateNotificationFeature(IFeatureDefinitionContext context)
        {
            var notificationFeature = context.Create(
                            AppFeatures.Notification,
                            defaultValue: "false",
                            displayName: L("Features.Notification"),
                            description: L("Features.Notification.Description"),
                            inputType: new CheckboxInputType());

            notificationFeature[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
            {
                TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                IsVisibleOnPricingTable = true
            };

            notificationFeature.CreateChildFeature(
                AppFeatures.SmsNotification,
                defaultValue: "false",
                displayName: L("Features.SmsNotification"),
                description: L("Features.SmsNotification.Description"),
               inputType: new CheckboxInputType()
               )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
               {
                   TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                   IsVisibleOnPricingTable = true
               };

            notificationFeature.CreateChildFeature(
               AppFeatures.EmailNotification,
               defaultValue: "false",
               displayName: L("Features.EmailNotification"),
               description: L("Features.EmailNotification.Description"),
               inputType: new CheckboxInputType()
               )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
               {
                   TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                   IsVisibleOnPricingTable = true
               };

            notificationFeature.CreateChildFeature(
                AppFeatures.WeChatNotification,
                defaultValue: "false",
                displayName: L("Features.WeChatNotification"),
                description: L("Features.WeChatNotification.Description"),
                inputType: new CheckboxInputType()
                )[FeatureMetadata.CustomFeatureKey] = new FeatureMetadata
                {
                    TextHtmlColor = value => value == "true" ? "#5cb85c" : "#d9534f",
                    IsVisibleOnPricingTable = true
                };
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, VappsConsts.ServerSideLocalizationSourceName);
        }
    }
}
