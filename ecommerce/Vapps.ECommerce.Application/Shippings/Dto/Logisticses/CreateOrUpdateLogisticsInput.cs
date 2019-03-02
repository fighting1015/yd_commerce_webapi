using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Shippings.Dto
{
    public class CreateOrUpdateLogisticsInput
    {
        /// <summary>
        /// 物流Id
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 快递 Key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 快递简写
        /// </summary>
        public virtual string Memo { get; set; }

        /// <summary>
        /// 排序(用户绑定物流可以自定义排序)
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
