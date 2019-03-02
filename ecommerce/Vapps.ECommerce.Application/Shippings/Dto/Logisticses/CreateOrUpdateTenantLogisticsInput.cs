namespace Vapps.ECommerce.Shippings.Dto.Logisticses
{
    public class CreateOrUpdateTenantLogisticsInput
    {
        /// <summary>
        /// 自选物流Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 平台物流Id
        /// </summary>
        public int LogisticsId { get; set; }

        /// <summary>
        /// 排序(用户绑定物流可以自定义排序)
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
