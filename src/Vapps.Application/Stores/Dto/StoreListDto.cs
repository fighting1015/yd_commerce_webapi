using Vapps.Orders;

namespace Vapps.Stores.Dto
{
    public class StoreListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 店铺名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource OrderSourceType { get; set; }

        /// <summary>
        /// 订单同步
        /// </summary>
        public bool OrderSync { get; set; }
    }
}
