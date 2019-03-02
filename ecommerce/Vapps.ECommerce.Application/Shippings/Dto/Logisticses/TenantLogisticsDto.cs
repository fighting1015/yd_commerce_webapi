using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Shippings.Dto.Logisticses
{
    public class TenantLogisticsDto : EntityDto
    {
        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
