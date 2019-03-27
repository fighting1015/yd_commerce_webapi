using Abp;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Timing;
using System.Threading.Tasks;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Events;

namespace Vapps.ECommerce.Orders
{
    public class OrderProcessingManager : VappsDomainServiceBase, IOrderProcessingManager
    {
        private readonly IShipmentManager _shipmentManager;
        private readonly IOrderManager _orderManager;
        private readonly IEventBus _eventBus;

        public OrderProcessingManager(IShipmentManager shipmentManager,
            IOrderManager orderManager,
            IEventBus eventBus)
        {
            this._shipmentManager = shipmentManager;
            this._orderManager = orderManager;
            this._eventBus = eventBus;
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public virtual async Task ShipAsync(Shipment shipment, bool notifyCustomer)
        {
            if (shipment == null)
                throw new AbpException("shipment");

            var order = await _orderManager.GetByIdAsync(shipment.OrderId);
            if (order == null)
                throw new AbpException("Order cannot be loaded");

            if (shipment.Status != ShippingStatus.NotYetShipped)
            {
                await _eventBus.TriggerAsync(new ShipmentSentEvent(shipment));
                return;
            }

            if (shipment.Id == 0)
                await _shipmentManager.CreateAsync(shipment);

            //process products with "Multiple warehouse" support enabled
            //foreach (var item in shipment.Items)
            //{
            //    if (item.OrderItemId == 0)
            //    {
            //        continue;
            //    }

            //    //var orderItem = _orderManager.GetOrderItemByIdAsync(item.OrderItemId);

            //    // TODO：减少库存
            //}

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);
            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Shipments);

            //check whether we have more items to ship
            if (order.HasItemsToAddToShipment(_shipmentManager) || order.HasItemsToShip())
                order.ShippingStatus = ShippingStatus.PartiallyShipped;
            else
                order.ShippingStatus = ShippingStatus.Taked;

            await _orderManager.UpdateAsync(order);

            if (notifyCustomer)
            {
                //发货通知
            }

            //触发发货事件
            await _eventBus.TriggerAsync(new ShipmentSentEvent(shipment));

            //修改订单状态
            //await CheckOrderStatus(order);
        }

        /// <summary>
        /// Checks order status
        /// 检查订单状态
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Validated order</returns>
        public virtual async Task CheckOrderStatus(Order order)
        {
            if (order == null)
                throw new AbpException("order");

            if (order.PaymentStatus == PaymentStatus.Paid && !order.PaidOn.HasValue)
            {
                //ensure that paid date is set
                order.PaidOn = Clock.Now;
                await _orderManager.UpdateAsync(order);
            }

            if (order.OrderStatus == OrderStatus.WaitConfirm)
            {
                if (order.PaymentStatus == PaymentStatus.Paid)
                {
                    await SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus == OrderStatus.WaitConfirm)
            {
                if (order.ShippingStatus != ShippingStatus.Received &&
                    order.ShippingStatus != ShippingStatus.IssueWithReject)
                {
                    await SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus != OrderStatus.Canceled &&
                order.OrderStatus != OrderStatus.Completed)
            {
                // 已付款订单,快递状态改为已收件
                if (order.PaymentStatus == PaymentStatus.Paid)
                {
                    var completed = false;
                    if (order.ShippingStatus == ShippingStatus.NotRequired)
                    {
                        //shipping is not required
                        completed = true;
                    }
                    else
                    {
                        //shipping is required
                        completed = order.ShippingStatus == ShippingStatus.Received;
                    }

                    if (completed)
                    {
                        await SetOrderStatus(order, OrderStatus.Completed, true);
                    }
                }
            }
        }

        /// <summary>
        /// 设置订单状态
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="os">New order status</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public virtual async Task SetOrderStatus(Order order, OrderStatus os, bool notifyCustomer)
        {
            #region 验证参数

            if (order == null)
                throw new AbpException("order");

            OrderStatus prevOrderStatus = order.OrderStatus;
            if (prevOrderStatus == os)
                return;

            #endregion

            //设置并保存订单状态
            order.OrderStatus = os;
            await _orderManager.UpdateAsync(order);

            //订单完成
            if (prevOrderStatus != OrderStatus.Completed && os == OrderStatus.Completed)
            {
                // TODO:计算累计消费额
                // TODO:计算商品销量

                if (notifyCustomer)
                {
                    //notification==通知

                }
            }

            // 订单取消
            if (prevOrderStatus == OrderStatus.Completed &&
                os == OrderStatus.Canceled)
            {
                // TODO:计算累计消费额
                // TODO:计算商品销量

                //notification==通知
                if (notifyCustomer)
                {

                }
            }
        }

        ///// <summary>
        ///// 对商品增加库存（下单）
        ///// </summary>
        ///// <returns></returns>
        //private List<WarehouseMapQuantityDetail> ProductAdjustInvetory(Product product, int quantity, string attributesXmls, int currentWarehouseId, string message = "")
        //{
        //    //简单商品
        //        var adjustInventoryResult = _productService.AdjustInventory(product, quantity, attributesXmls, currentWarehouseId, null, message);
        //        return adjustInventoryResult;

        //}
    }
}
