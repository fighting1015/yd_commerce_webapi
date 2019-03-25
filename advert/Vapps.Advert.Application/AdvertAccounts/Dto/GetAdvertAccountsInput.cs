using Abp.Runtime.Validation;
using Vapps.Dto;

namespace Vapps.Advert.AdvertAccounts.Dto
{
    public class GetAdvertAccountsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public AdvertChannel[] AdvertChannels { get; set; }

        /// <summary>
        /// 第三方Id
        /// </summary>
        public string ThirdpartyId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime,Id DESC";
            }
        }
    }
}
