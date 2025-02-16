using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;

namespace Bazingo_Core.DomainLogic
{
    public class OrderManager
    {
        public bool ValidateOrder(OrderEntity order)
        {
            if (order == null)
                return false;

            if (order.OrderItems == null || !order.OrderItems.Any())
                return false;

            if (string.IsNullOrWhiteSpace(order.UserId))
                return false;

            if (order.TotalAmount <= 0)
                return false;

            return true;
        }

        public decimal CalculateOrderTotal(IEnumerable<OrderItemEntity> items)
        {
            if (items == null || !items.Any())
                return 0;

            return items.Sum(item => item.Quantity * item.Price);
        }

        public bool CanCancelOrder(OrderEntity order)
        {
            if (order == null)
                return false;

            // Can only cancel orders that are pending or processing
            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Processing)
                return false;

            // Cannot cancel orders after 24 hours
            var timeSinceOrder = DateTime.UtcNow - order.OrderDate;
            if (timeSinceOrder.TotalHours > 24)
                return false;

            return true;
        }

        public bool CanRefundOrder(OrderEntity order)
        {
            if (order == null)
                return false;

            // Can only refund completed orders
            if (order.Status != OrderStatus.Delivered)
                return false;

            // Cannot refund orders after 30 days
            var deliveryDate = GetDeliveryDate(order);
            if (!deliveryDate.HasValue)
                return false;

            var timeSinceDelivery = DateTime.UtcNow - deliveryDate.Value;
            if (timeSinceDelivery.TotalDays > 30)
                return false;

            return true;
        }

        private DateTime? GetDeliveryDate(OrderEntity order)
        {
            return order.StatusHistory?
                .Where(h => h.Status == OrderStatus.Delivered)
                .OrderByDescending(h => h.ChangedDate)
                .FirstOrDefault()?.ChangedDate;
        }

        public OrderStatus GetNextStatus(OrderEntity order)
        {
            switch (order.Status)
            {
                case OrderStatus.Pending:
                    return OrderStatus.Processing;
                case OrderStatus.Processing:
                    return OrderStatus.Shipped;
                case OrderStatus.Shipped:
                    return OrderStatus.Delivered;
                case OrderStatus.Delivered:
                    return order.Status; // Delivered is the final state
                default:
                    return order.Status;
            }
        }

        public IEnumerable<OrderStatusHistoryEntity> GetOrderTimeline(OrderEntity order)
        {
            if (order == null || order.StatusHistory == null)
                return Enumerable.Empty<OrderStatusHistoryEntity>();

            return order.StatusHistory
                .OrderBy(h => h.ChangedDate)
                .ToList();
        }

        public bool IsOrderOverdue(OrderEntity order)
        {
            if (order == null)
                return false;

            var expectedDeliveryDate = order.OrderDate.AddDays(7); // Default to 7 days delivery time
            return DateTime.UtcNow > expectedDeliveryDate;
        }
    }
}
