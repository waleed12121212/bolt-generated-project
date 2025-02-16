using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Application.DTOs.Order;
using Bazingo_Core.Entities;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Enums;
using Bazingo_Application.DTOs.Shipping;

namespace Bazingo_Application.Interfaces
{
    public interface IOrderService
    {
        Task<Bazingo_Core.Models.Common.ApiResponse<List<OrderDto>>> GetOrdersAsync();
        Task<Bazingo_Core.Models.Common.ApiResponse<OrderDto>> GetOrderByIdAsync(int id);
        Task<Bazingo_Core.Models.Common.ApiResponse<List<OrderDto>>> GetOrdersByBuyerIdAsync(string buyerId);
        Task<Bazingo_Core.Models.Common.ApiResponse<List<OrderDto>>> GetOrdersByUserIdAsync(string userId);
        Task<Bazingo_Core.Models.Common.ApiResponse<List<OrderDto>>> GetOrdersBySellerIdAsync(string sellerId);
        Task<Bazingo_Core.Models.Common.ApiResponse<List<OrderDto>>> GetOrdersByStatusAsync(OrderStatus status);
        Task<Bazingo_Core.Models.Common.ApiResponse<OrderDto>> CreateOrderAsync(CreateOrderDto orderDto, string buyerId);
        Task<Bazingo_Core.Models.Common.ApiResponse<OrderDto>> UpdateOrderStatusAsync(UpdateOrderStatusDto updateDto);
        Task<Bazingo_Core.Models.Common.ApiResponse<bool>> UpdatePaymentStatusAsync(int orderId, PaymentStatus status);
        Task<Bazingo_Core.Models.Common.ApiResponse<bool>> CancelOrderAsync(int orderId, string reason);
        Task<Bazingo_Core.Models.Common.ApiResponse<bool>> UpdateShippingStatusAsync(int orderId, string status);
        Task<Bazingo_Core.Models.Common.ApiResponse<bool>> UpdateShippingStatusAsync(UpdateShippingStatusDto dto);
    }
}
