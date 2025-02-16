using Bazingo_Application.DTOs.Users;
using Bazingo_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bazingo_Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Bazingo_Application.Interfaces;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserApplicationService _userService;
        private readonly IOrderService _orderService;

        public AdminController(IUserApplicationService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users.Select(u => new UserProfileDTO
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            }));
        }

        [HttpPut("block-user/{userId}")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            user.IsVerified = false;
            await _userService.UpdateUserAsync(user);

            return Ok(new { message = "User blocked successfully." });
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetOrdersAsync();
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
