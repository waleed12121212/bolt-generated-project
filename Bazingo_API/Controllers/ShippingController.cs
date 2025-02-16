using Bazingo_Application.DTOs.Shipping;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bazingo_Core.Enums;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShippingController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<ShippingController> _logger;

        public ShippingController(IOrderService orderService , ILogger<ShippingController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ApiResponse<ShippingDTO>>> GetShippingDetails(int orderId)
        {
            var orderResponse = await _orderService.GetOrderByIdAsync(orderId);
            if (!orderResponse.Succeeded || orderResponse.Data == null)
                return NotFound(new ApiResponse<ShippingDTO> { Message = "Order not found" });

            if (orderResponse.Data.ShippingInfo == null)
                return NotFound(new ApiResponse<ShippingDTO> { Message = "Shipping information not found for this order" });

            return Ok(new ApiResponse<ShippingDTO>
            {
                Data = orderResponse.Data.ShippingInfo ,
                Message = "Shipping details retrieved successfully" ,
                Succeeded = true
            });
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateShippingStatus(int orderId , [FromBody] UpdateShippingDto dto)
        {
            try
            {
                var orderResponse = await _orderService.GetOrderByIdAsync(orderId);
                if (!orderResponse.Succeeded || orderResponse.Data == null || orderResponse.Data.ShippingInfo == null)
                    return NotFound(new ApiResponse<bool> { Message = "Order or shipping information not found" });

                var result = await _orderService.UpdateShippingStatusAsync(orderId , dto.Status.ToString());

                if (!result.Succeeded)
                    return BadRequest(new ApiResponse<bool> { Message = "Failed to update shipping status" });

                return Ok(new ApiResponse<bool>
                {
                    Data = true ,
                    Message = "Shipping status updated successfully" ,
                    Succeeded = true
                });
            }
            catch (ArgumentException)
            {
                return BadRequest(new ApiResponse<bool> { Message = "Invalid shipping status value" });
            }
        }

        [HttpPut("status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateShippingStatus([FromBody] UpdateShippingStatusDto shippingStatusDto)
        {
            var result = await _orderService.UpdateShippingStatusAsync(
                shippingStatusDto.OrderId ,
                shippingStatusDto.Status
            );

            if (!result.Succeeded)
                return BadRequest(new ApiResponse<bool> { Message = "Failed to update shipping status" });

            return Ok(new ApiResponse<bool>
            {
                Data = true ,
                Message = "Shipping status updated successfully" ,
                Succeeded = true
            });
        }
    }
}
