using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.Pictures.Dto
{
    public class GetPictureInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 图片分组Id，-1 获取全部
        /// </summary>
        public int GroupId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
