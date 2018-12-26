using System.Collections.Generic;

namespace Vapps.Editions.Dto
{
    public class EditionsSelectOutput
    {
        public EditionsSelectOutput()
        {
            AllFeatures = new List<FlatFeatureSelectDto>();
            EditionsWithFeatures = new List<EditionWithFeaturesForSelectDto>();
        }

        /// <summary>
        /// 所有功能（展示用）
        /// </summary>
        public List<FlatFeatureSelectDto> AllFeatures { get; set; }

        /// <summary>
        /// 版本及版本的功能
        /// </summary>
        public List<EditionWithFeaturesForSelectDto> EditionsWithFeatures { get; set; }

        public int? TenantEditionId { get; set; }
    }
}
