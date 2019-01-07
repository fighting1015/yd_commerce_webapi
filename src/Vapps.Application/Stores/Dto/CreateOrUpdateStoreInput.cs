using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Vapps.Orders;

namespace Vapps.Stores.Dto
{
    //[AutoMapFrom(typeof(Store))]
    public class CreateOrUpdateStoreInput
    {
        /// <summary>
        /// Id，空或者为0时创建店铺
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 店铺名
        /// </summary>
        [Required]
        [StringLength(Store.MaxNameFieldLength)]
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
    }
}
