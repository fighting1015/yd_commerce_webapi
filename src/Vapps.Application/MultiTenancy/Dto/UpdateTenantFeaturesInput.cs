using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Vapps.MultiTenancy.Dto
{
    public class UpdateTenantFeaturesInput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// 特性
        /// </summary>
        [Required]
        public List<NameValueDto> FeatureValues { get; set; }
    }
}