using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Pictures.Dto
{
    public class BatchMove2GroupInput
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 需要转移的图片Id
        /// </summary>
        [Required]
        public List<long> Ids { get; set; }
    }
}
