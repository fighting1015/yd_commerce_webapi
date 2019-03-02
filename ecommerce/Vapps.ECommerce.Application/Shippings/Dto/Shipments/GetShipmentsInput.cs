using Abp.Runtime.Validation;
using System;
using Vapps.Dto;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class GetShipmentsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 空(全部)
        /// </summary>
        public ShippingStatus? Status { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 发货时间(开始时间-UTC)
        /// </summary>
        public DateTime? DeliveryFrom { get; set; }

        /// <summary>
        /// 发货时间(结束时间-UTC)
        /// </summary>
        public DateTime? DeliveryTo { get; set; }

        /// <summary>
        /// 签收时间(开始时间-UTC)
        /// </summary>
        public DateTime? ReceivedFrom { get; set; }

        /// <summary>
        /// 签收时间(结束时间-UTC)
        /// </summary>
        public DateTime? ReceivedTo { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
