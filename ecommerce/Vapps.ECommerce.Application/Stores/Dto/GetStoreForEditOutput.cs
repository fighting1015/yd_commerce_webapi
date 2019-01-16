using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using Vapps.Orders;

namespace Vapps.ECommerce.Stores.Dto
{
    //[AutoMapFrom(typeof(Store))]
    public class GetStoreForEditOutput : EntityDto
    {
        /// <summary>
        /// 店铺名
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// Url
        ///// </summary>
        //public  string Url { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// 第三方App id
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 第三方App secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource OrderSourceType { get; set; }

        /// <summary>
        /// 订单同步
        /// </summary>
        public bool OrderSync { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModificationTime { get; set; }
    }
}
