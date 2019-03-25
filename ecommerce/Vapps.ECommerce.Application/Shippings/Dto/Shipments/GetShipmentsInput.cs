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
        /// 发货时间(Utc)
        /// </summary>
        public DateRangeDto DeliveriedOn { get; set; }

        /// <summary>
        /// 签收时间(Utc)
        /// </summary>
        public DateRangeDto ReceivedOn { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id DESC";
            }
        }
    }
}
