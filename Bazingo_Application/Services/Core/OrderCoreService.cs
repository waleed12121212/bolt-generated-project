using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Application.Services.Core
{
    public class OrderCoreService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderCoreService> _logger;

        public OrderCoreService(IUnitOfWork unitOfWork, ILogger<OrderCoreService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OrderEntity> GetOrderByIdAsync(int orderId)
        {
            try
            {
                return await _unitOfWork.Orders.GetByIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID {OrderId}", orderId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderEntity>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                return await _unitOfWork.Orders.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
                throw;
            }
        }

        public async Task<OrderEntity> CreateOrderAsync(string userId)
        {
            try
            {
                var order = new OrderEntity
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending
                };

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.CompleteAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}", userId);
                throw;
            }
        }

        public async Task<OrderEntity> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found");
                }

                if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
                {
                    throw new ArgumentException($"Invalid order status: {status}");
                }

                order.Status = orderStatus;
                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.CompleteAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId}", orderId);
                throw;
            }
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return false;
                }

                order.Status = OrderStatus.Cancelled;
                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
                throw;
            }
        }
    }
}
