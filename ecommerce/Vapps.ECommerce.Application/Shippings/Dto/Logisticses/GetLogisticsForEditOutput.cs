using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Shippings.Dto
{
    public class GetLogisticsForEditOutput : EntityDto
    {
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
    }
}
