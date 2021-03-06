﻿using Abp.Application.Features;
using Abp.AutoMapper;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Vapps.Editions.Dto
{
    [AutoMap(typeof(Feature))]
    public class FlatFeatureSelectDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public IInputType InputType { get; set; }

        public string TextHtmlColor { get; set; }
    }
}