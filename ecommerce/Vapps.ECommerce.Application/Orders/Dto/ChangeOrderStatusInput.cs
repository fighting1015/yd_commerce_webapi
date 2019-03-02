using Abp.Application.Services.Dto;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders.Dto
{
    public class ChangeOrderStatusInput<T> : EntityDto<long>
    {
        /// <summary>
        /// id数组
        /// </summary>
        public T[] Ids { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public T Stauts { get; set; }
    }
}
