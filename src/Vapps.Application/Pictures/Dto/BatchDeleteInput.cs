using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Pictures.Dto
{
    public class BatchDeleteInput
    {
        /// <summary>
        /// 需要删除的图片Id
        /// </summary>
        [Required]
        public List<long> Ids { get; set; }
    }
}
