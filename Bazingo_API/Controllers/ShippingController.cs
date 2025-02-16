    using Bazingo_Application.DTOs.Shipping;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Enums;
    using Microsoft.Extensions.Logging;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class ShippingController : ControllerBase
        {
            private readonly IOrderService _orderService;
            private readonly ILogger<ShippingController> _logger;

            public ShippingController(IOrderService orderService, ILogger<ShippingController> logger)
            {
                _orderService = orderService;
                _logger = logger;
            }

            [HttpGet("{orderId}")]
            public async Task<ActionResult<ApiResponse<ShippingDTO>>> GetShippingDetails(int orderId)
            {
                try
                {
                    var orderResponse = await _orderService.GetOrderByIdAsync(orderId);
                    if (!orderResponse.Succeeded || orderResponse.Data == null)
                        return NotFound(new ApiResponse<ShippingDTO> { Message = "Order not found" });

                    if (orderResponse.Data.ShippingInfo == null)
                        return NotFound(new ApiResponse<ShippingDTO> { Message = "Shipping information not found for this order" });

                    return Ok(new ApiResponse<ShippingDTO>
                    {
                        Data = orderResponse.Data.ShippingInfo,
                        Message = "Shipping details retrieved successfully",
                        Succeeded = true
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting shipping details for order ID: {OrderId}", orderId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("{orderId}/status")]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<ActionResult<ApiResponse<bool>>> UpdateShippingStatus(int orderId, [FromBody] UpdateShippingDto dto)
            {
                if (dto == null)
                {
                    return BadRequest("UpdateShippingDto object is required.");
                }

                try
                {
                    var orderResponse = await _orderService.GetOrderByIdAsync(orderId);
                    if (!orderResponse.Succeeded || orderResponse.Data == null || orderResponse.Data.ShippingInfo == null)
                        return NotFound(new ApiResponse<bool> { Message = "Order or shipping information not found" });

                    var result = await _orderService.UpdateShippingStatusAsync(orderId, dto.Status.ToString());

                    if (!result.Succeeded)
                        return BadRequest(new ApiResponse<bool> { Message = "Failed to update shipping status" });

                    return Ok(new ApiResponse<bool>
                    {
                        Data = true,
                        Message = "Shipping status updated successfully",
                        Succeeded = true
                    });
                }
                catch (ArgumentException)
                {
                    return BadRequest(new ApiResponse<bool> { Message = "Invalid shipping status value" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating shipping status for order ID: {OrderId}", orderId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("status")]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<ActionResult<ApiResponse<bool>>> UpdateShippingStatus([FromBody] UpdateShippingStatusDto shippingStatusDto)
            {
                if (shippingStatusDto == null)
                {
                    return BadRequest("UpdateShippingStatusDto object is required.");
                }

                try
                {
                    var result = await _orderService.UpdateShippingStatusAsync(
                        shippingStatusDto.OrderId,
                        shippingStatusDto.Status
                    );

                    if (!result.Succeeded)
                        return BadRequest(new ApiResponse<bool> { Message = "Failed to update shipping status" });

                    return Ok(new ApiResponse<bool>
                    {
                        Data = true,
                        Message = "Shipping status updated successfully",
                        Succeeded = true
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating shipping status");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
