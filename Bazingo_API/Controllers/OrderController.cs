using Bazingo_Application.DTOs.Order;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bazingo_Core.Enums;
using System.Security.Claims;

namespace Bazingo_API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<OrderDto>>> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("user")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpGet("seller")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetSellerOrders()
        {
            var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.GetOrdersBySellerIdAsync(sellerId);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetOrdersByStatus(OrderStatus status)
        {
            var result = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.CreateOrderAsync(dto, userId);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto);
            return result != null 
                ? Ok(new { message = "Order status updated successfully" }) 
                : BadRequest(new { message = "Failed to update order status" });
        }

        [HttpPut("{id}/payment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<OrderDto>>> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var paymentStatus = Enum.Parse<PaymentStatus>(status);
            var result = await _orderService.UpdatePaymentStatusAsync(id, paymentStatus);
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<bool>>> CancelOrder(int id, [FromBody] string reason)
        {
            var result = await _orderService.CancelOrderAsync(id, reason);
            return Ok(result);
        }
    }
}
