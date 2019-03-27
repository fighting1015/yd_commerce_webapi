using Abp.Application.Services.Dto;

namespace Vapps.Advert.AdvertAccounts.Dto
{
    public class GetAdvertAccountForEditOutput : NullableIdDto<long>
    {
        /// <summary>
        /// 店铺（必填）
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 第三方Id
        /// </summary>
        public string ThirdpartyId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public AdvertChannel Channel { get; set; }

        /// <summary>
        /// 数据自动同步
        /// </summary>
        public bool DataAutoSync { get; set; }

    }
}
