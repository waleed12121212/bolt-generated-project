using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;
using Bazingo_Core.Models.Common;
using Bazingo_Application.DTOs.Order;
using Bazingo_Application.DTOs.Shipping;
using Bazingo_Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bazingo_Application.Services
{
    public class OrderApplicationService : Bazingo_Application.Interfaces.IOrderService
    {
        private readonly Bazingo_Core.Interfaces.IOrderService _orderService;
        private readonly ILogger<OrderApplicationService> _logger;

        public OrderApplicationService(Bazingo_Core.Interfaces.IOrderService orderService, ILogger<OrderApplicationService> logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersAsync()
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(null);
                if (orders == null)
                    return ApiResponse<List<OrderDto>>.CreateError("No orders found");

                var orderDtos = orders.Select(MapToOrderDto).ToList();
                return ApiResponse<List<OrderDto>>.CreateSuccess(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders");
                return ApiResponse<List<OrderDto>>.CreateError("Error getting orders");
            }
        }

        public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return ApiResponse<OrderDto>.CreateError($"Order with id {id} not found");

                var orderDto = MapToOrderDto(order);
                return ApiResponse<OrderDto>.CreateSuccess(orderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by id {OrderId}", id);
                return ApiResponse<OrderDto>.CreateError($"Error getting order with id {id}");
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersByBuyerIdAsync(string buyerId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(buyerId);
                if (orders == null)
                    return ApiResponse<List<OrderDto>>.CreateError($"No orders found for buyer {buyerId}");

                var orderDtos = orders.Select(MapToOrderDto).ToList();
                return ApiResponse<List<OrderDto>>.CreateSuccess(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for buyer {BuyerId}", buyerId);
                return ApiResponse<List<OrderDto>>.CreateError($"Error getting orders for buyer {buyerId}");
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                if (orders == null)
                    return ApiResponse<List<OrderDto>>.CreateError($"No orders found for user {userId}");

                var orderDtos = orders.Select(MapToOrderDto).ToList();
                return ApiResponse<List<OrderDto>>.CreateSuccess(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
                return ApiResponse<List<OrderDto>>.CreateError($"Error getting orders for user {userId}");
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersBySellerIdAsync(string sellerId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(sellerId);
                if (orders == null)
                    return ApiResponse<List<OrderDto>>.CreateError($"No orders found for seller {sellerId}");

                var orderDtos = orders.Select(MapToOrderDto).ToList();
                return ApiResponse<List<OrderDto>>.CreateSuccess(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for seller {SellerId}", sellerId);
                return ApiResponse<List<OrderDto>>.CreateError($"Error getting orders for seller {sellerId}");
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(null);
                if (orders == null)
                    return ApiResponse<List<OrderDto>>.CreateError($"No orders found with status {status}");

                var filteredOrders = orders.Where(o => o.Status == status);
                var orderDtos = filteredOrders.Select(MapToOrderDto).ToList();
                return ApiResponse<List<OrderDto>>.CreateSuccess(orderDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders with status {Status}", status);
                return ApiResponse<List<OrderDto>>.CreateError($"Error getting orders with status {status}");
            }
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto orderDto, string buyerId)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(buyerId);
                if (order == null)
                    return ApiResponse<OrderDto>.CreateError("Failed to create order");

                return ApiResponse<OrderDto>.CreateSuccess(MapToOrderDto(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for buyer {BuyerId}", buyerId);
                return ApiResponse<OrderDto>.CreateError("Error creating order");
            }
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(UpdateOrderStatusDto updateDto)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(updateDto.OrderId, updateDto.Status.ToString());
                if (order == null)
                    return ApiResponse<OrderDto>.CreateError($"Failed to update order status for order {updateDto.OrderId}");

                return ApiResponse<OrderDto>.CreateSuccess(MapToOrderDto(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status for order {OrderId}", updateDto.OrderId);
                return ApiResponse<OrderDto>.CreateError($"Error updating order status");
            }
        }

        public async Task<ApiResponse<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus status)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                    return ApiResponse<bool>.CreateError("Order not found");

                order.PaymentStatus = status;
                await _orderService.UpdateOrderStatusAsync(orderId, order.Status.ToString());
                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status for order {OrderId}", orderId);
                return ApiResponse<bool>.CreateError($"Error updating payment status");
            }
        }

        public async Task<ApiResponse<bool>> CancelOrderAsync(int orderId, string reason)
        {
            try
            {
                var success = await _orderService.CancelOrderAsync(orderId);
                if (!success)
                    return ApiResponse<bool>.CreateError("Failed to cancel order");

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
                return ApiResponse<bool>.CreateError($"Error cancelling order");
            }
        }

        public async Task<ApiResponse<bool>> UpdateShippingStatusAsync(int orderId, string status)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Shipped.ToString());
                if (order == null)
                    return ApiResponse<bool>.CreateError("Order not found");

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating shipping status for order {OrderId}", orderId);
                return ApiResponse<bool>.CreateError($"Error updating shipping status");
            }
        }

        public async Task<ApiResponse<bool>> UpdateShippingStatusAsync(UpdateShippingStatusDto dto)
        {
            return await UpdateShippingStatusAsync(dto.OrderId, dto.Status);
        }

        private OrderDto MapToOrderDto(OrderEntity order)
        {
            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                ShippingAddress = order.ShippingAddress,
                OrderItems = order.OrderItems?.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Price * item.Quantity
                }).ToList() ?? new List<OrderItemDto>()
            };
        }
    }
}
