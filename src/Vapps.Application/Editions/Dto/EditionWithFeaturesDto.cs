using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Vapps.Editions.Dto
{
    public class EditionWithFeaturesDto
    {
        public EditionWithFeaturesDto()
        {
            FeatureValues = new List<NameValueDto>();
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public EditionSelectDto Edition { get; set; }

        /// <summary>
        /// 版本功能
        /// </summary>
        public List<NameValueDto> FeatureValues { get; set; }
    }

    public class EditionWithFeaturesForSelectDto
    {
        public EditionWithFeaturesForSelectDto()
        {
            FeatureValues = new List<FeatureValueDto>();
        }

        /// <summary>
        /// 版本信息
        /// </summary>
        public EditionSelectDto Edition { get; set; }

        /// <summary>
        /// 版本功能
        /// </summary>
        public List<FeatureValueDto> FeatureValues { get; set; }
    }

    public class FeatureValueDto : NameValueDto
    {
        public FeatureValueDto()
        {

        }

        public FeatureValueDto(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 版本功能
        /// </summary>
        public List<NameValueDto> Childs { get; set; }
    }
}

