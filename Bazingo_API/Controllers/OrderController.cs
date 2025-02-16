    using Bazingo_Application.DTOs.Order;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Enums;
    using System.Security.Claims;
    using Bazingo_Core;

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
                try
                {
                    var result = await _orderService.GetOrderByIdAsync(id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting order by ID: {OrderId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("user")]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetUserOrders()
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var result = await _orderService.GetOrdersByUserIdAsync(userId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user orders");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("seller")]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetSellerOrders()
            {
                try
                {
                    var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var result = await _orderService.GetOrdersBySellerIdAsync(sellerId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting seller orders");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("status/{status}")]
            [Authorize(Roles = Constants.Roles.Admin + "," + Constants.Roles.Seller)]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<IEnumerable<OrderDto>>>> GetOrdersByStatus(OrderStatus status)
            {
                try
                {
                    var result = await _orderService.GetOrdersByStatusAsync(status);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting orders by status: {Status}", status);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto dto)
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var result = await _orderService.CreateOrderAsync(dto, userId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating order");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut]
            [Authorize(Roles = Constants.Roles.Admin + "," + Constants.Roles.Seller)]
            public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto updateOrderStatusDto)
            {
                try
                {
                    var result = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto);
                    return result != null
                        ? Ok(new { message = "Order status updated successfully" })
                        : BadRequest(new { message = "Failed to update order status" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating order status");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("{id}/payment")]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<OrderDto>>> UpdatePaymentStatus(int id, [FromBody] string status)
            {
                try
                {
                    var paymentStatus = Enum.Parse<PaymentStatus>(status);
                    var result = await _orderService.UpdatePaymentStatusAsync(id, paymentStatus);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating payment status");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("{id}/cancel")]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<bool>>> CancelOrder(int id, [FromBody] string reason)
            {
                try
                {
                    var result = await _orderService.CancelOrderAsync(id, reason);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling order");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
