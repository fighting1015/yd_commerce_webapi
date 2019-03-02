using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using Vapps.Dto;
using Vapps.ECommerce.Orders.Dto;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;

namespace Vapps.ECommerce.Orders
{
    public interface IOrderAppService
    {
        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<OrderListDto>> GetOrders(GetOrdersInput input);

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<OrderDetailDto> GetOrderDetail(long orderId);

        /// <summary>
        /// 创建或更新订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateOrder(CreateOrUpdateOrderInput input);

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeOrderStatus(ChangeOrderStatusInput<OrderStatus> input);

        /// <summary>
        /// 修改物流状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangeShippingStatus(ChangeOrderStatusInput<ShippingStatus> input);

        /// <summary>
        /// 修改付款状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ChangePaymentStatus(ChangeOrderStatusInput<PaymentStatus> input);

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteOrder(BatchInput<long> input);
    }
}
