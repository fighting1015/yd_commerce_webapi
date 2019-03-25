using Abp.Application.Services.Dto;
using System;

namespace Vapps.Advert.AdvertAccounts.Dto
{
    public class AdvertAccountListDto : EntityDto<long>
    {
        /// <summary>
        /// 第三方Id
        /// </summary>
        public string ThirdpartyId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 数据自动同步
        /// </summary>
        public bool DataAutoSync { get; set; }

        /// <summary>
        /// 历史消耗
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 总下单数
        /// </summary>
        public decimal TotalOrder { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
