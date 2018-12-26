using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Editions.Dto
{
    public class EditionsViewOutput
    {
        public EditionsViewOutput()
        {
            AllFeatures = new List<FlatFeatureSelectDto>();
        }

        /// <summary>
        /// 所有功能（展示用）
        /// </summary>
        public List<FlatFeatureSelectDto> AllFeatures { get; set; }

        /// <summary>
        /// 版本及版本的功能
        /// </summary>
        public EditionWithFeaturesDto EditionWithFeatures { get; set; }
    }
}
