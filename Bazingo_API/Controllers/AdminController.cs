    using Bazingo_Application.DTOs.Users;
    using Bazingo_Application.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Entities.Identity;
    using Microsoft.AspNetCore.Identity;
    using Bazingo_Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public class AdminController : ControllerBase
        {
            private readonly IUserApplicationService _userService;
            private readonly IOrderService _orderService;
            private readonly ILogger<AdminController> _logger;

            public AdminController(IUserApplicationService userService, IOrderService orderService, ILogger<AdminController> logger)
            {
                _userService = userService;
                _orderService = orderService;
                _logger = logger;
            }

            [HttpGet("users")]
            public async Task<IActionResult> GetAllUsers()
            {
                try
                {
                    var users = await _userService.GetAllUsersAsync();
                    return Ok(users.Select(u => new UserProfileDTO
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email
                    }));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all users");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("block-user/{userId}")]
            public async Task<IActionResult> BlockUser(string userId)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is required.");
                }

                try
                {
                    var user = await _userService.GetUserByIdAsync(userId);
                    if (user == null) return NotFound();

                    user.IsVerified = false;
                    await _userService.UpdateUserAsync(user);

                    return Ok(new { message = "User blocked successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error blocking user with ID: {UserId}", userId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("orders")]
            public async Task<IActionResult> GetAllOrders()
            {
                try
                {
                    var response = await _orderService.GetOrdersAsync();
                    if (!response.Succeeded)
                    {
                        return BadRequest(response);
                    }
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all orders");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
