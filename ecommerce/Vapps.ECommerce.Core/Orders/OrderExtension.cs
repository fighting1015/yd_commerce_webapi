using Abp.Domain.Repositories;
using Abp.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Shippings;
using Vapps.Extensions;
using Vapps.Helpers;

namespace Vapps.ECommerce.Orders
{

    public static class OrderExtension
    {
        /// <summary>
        /// 获取收件地址
        /// </summary>
        /// <param name="order"></param>
        public static string GetFullShippingAddress(this Order order)
        {
            return $"{order.ShippingProvince}{order.ShippingCity}{order.ShippingDistrict}{order.ShippingAddress}";
        }

        /// <summary>
        /// 重置订单Guid和Number
        /// </summary>
        /// <param name="order"></param>
        public static void ResetOrderGuidAndNumber(this Order order)
        {
            order.OrderNumber = Guid.NewGuid().ToLongId().ToString();
        }

        /// <summary>
        /// 生成订单号（4位毫秒+2位随机数）
        /// </summary>
        /// <param name="order"></param>
        public static void GenerateOrderNumber(this Order order)
        {
            string sarNum = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var randomNum = CommonHelper.GenerateRandomDigitCode(2);

            order.OrderNumber = string.Concat(sarNum, randomNum);
        }

        /// <summary>
        /// 生成订单号子
        /// </summary>
        /// <param name="orderItem"></param>
        public static void GenerateOrderItemNumber(this OrderItem orderItem)
        {
            var order = orderItem.Order;
            if (order == null)
                return;

            if (order.OrderNumber.IsNullOrWhiteSpace())
                order.GenerateOrderNumber();

            var index = orderItem.Order.Items.ToList().IndexOf(orderItem);

            orderItem.OrderItemNumber = string.Concat(orderItem.Order.OrderNumber, index);
        }

        /// <summary>
        /// 是否有待发货子订单
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether an order has items to ship</returns>
        public static bool HasItemsToShip(this Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            foreach (var orderItem in order.Items)
            {
                var totalNumberOfNotYetShippedItems = orderItem.GetTotalNumberOfNotYetShippedItems();
                if (totalNumberOfNotYetShippedItems <= 0)
                    continue;

                //yes, we have at least one item to ship
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否有可发货子订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="shipmentManager">Order</param>
        /// <returns></returns>
        public static bool HasItemsToAddToShipment(this Order order, IShipmentManager shipmentManager)
        {
            if (order == null)
                throw new ArgumentNullException("order");


            foreach (var orderItem in order.Items)
            {
                var totalNumberOfItemsCanBeAddedToShipment = orderItem.GetTotalNumberOfItemsCanBeAddedToShipment(shipmentManager);
                if (totalNumberOfItemsCanBeAddedToShipment <= 0)
                    continue;

                //是，可以发货
                return true;
            }
            return false;
        }

        /// <summary>
        /// 或者子订单最大可发货数量        
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <param name="shipmentManager"></param>
        /// <returns>未发货总数</returns>
        public static int GetTotalNumberOfItemsCanBeAddedToShipment(this OrderItem orderItem, IShipmentManager shipmentManager)
        {
            if (orderItem == null)
                throw new ArgumentNullException("orderItem");

            var totalInShipments = orderItem.GetTotalNumberOfItemsInAllShipment(shipmentManager);

            var qtyOrdered = orderItem.Quantity;
            var qtyCanBeAddedToShipmentTotal = qtyOrdered - totalInShipments;
            if (qtyCanBeAddedToShipmentTotal < 0)
                qtyCanBeAddedToShipmentTotal = 0;

            return qtyCanBeAddedToShipmentTotal;
        }

        /// <summary>
        /// 获取订单发货商品数量
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <param name="shipmentManager"></param>
        /// <returns>Total number of items in all shipmentss</returns>
        public static int GetTotalNumberOfItemsInAllShipment(this OrderItem orderItem, IShipmentManager shipmentManager)
        {
            if (orderItem == null)
                throw new ArgumentNullException("orderItem");

            var totalInShipments = 0;
            var shipments = orderItem.Order.Shipments.ToList();
            for (int i = 0; i < shipments.Count; i++)
            {
                var shipment = shipments[i];

                shipmentManager.ShipmentRepository.EnsureCollectionLoaded(shipment, s => s.Items);

                var si = shipment.Items
                    .FirstOrDefault(x => x.OrderItemId == orderItem.Id);
                if (si != null)
                {
                    totalInShipments += si.Quantity;
                }
            }
            return totalInShipments;
        }

        /// <summary>
        /// 获取未发货总数
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <returns>Total number of not yet shipped items (but added to shipments)</returns>
        public static int GetTotalNumberOfNotYetShippedItems(this OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException("orderItem");

            var result = 0;
            var shipments = orderItem.Order.Shipments.ToList();
            for (int i = 0; i < shipments.Count; i++)
            {
                var shipment = shipments[i];
                if (shipment.IsShiped())
                    //already shipped
                    continue;

                var si = shipment.Items
                    .FirstOrDefault(x => x.OrderItemId == orderItem.Id);
                if (si != null)
                {
                    result += si.Quantity;
                }
            }

            return result;
        }
    }
}
