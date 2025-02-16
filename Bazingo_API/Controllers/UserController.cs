    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Bazingo_Application.DTOs;
    using Bazingo_Core.Entities.Identity;
    using System.Linq;
    using Bazingo_Application.Interfaces;
    using Bazingo_Application.DTOs.Users;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserApplicationService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserApplicationService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Check if email is unique
                if (!await _userService.IsEmailUniqueAsync(userRegisterDTO.Email))
                {
                    return BadRequest(new { message = "Email is already in use." });
                }

                var user = new ApplicationUser
                {
                    FirstName = userRegisterDTO.FirstName,
                    LastName = userRegisterDTO.LastName,
                    Email = userRegisterDTO.Email,
                    UserName = userRegisterDTO.Email // Using email as username
                };

                // Since we removed AddUserAsync, we'll use UpdateUserAsync for creation
                var success = await _userService.UpdateUserAsync(user);
                if (!success)
                {
                    return BadRequest(new { message = "Failed to register user." });
                }

                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _userService.GetUserByEmailAsync(userLoginDTO.Email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Note: In a real application, you would validate the password here
                // and generate a JWT token for authentication

                return Ok(new { message = "User logged in successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                var userProfile = new UserProfileDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile for ID: {UserId}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("profile/{id}")]
        public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UpdateUserProfileDTO updateProfileDTO)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            if (updateProfileDTO == null)
            {
                return BadRequest("UpdateUserProfileDTO object is required.");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                // Check if email is being changed and if it's unique
                if (updateProfileDTO.Email != user.Email && !await _userService.IsEmailUniqueAsync(updateProfileDTO.Email, id))
                {
                    return BadRequest(new { message = "Email is already in use." });
                }

                user.FirstName = updateProfileDTO.FirstName;
                user.LastName = updateProfileDTO.LastName;
                user.Email = updateProfileDTO.Email;
                user.PhoneNumber = updateProfileDTO.PhoneNumber;

                var success = await _userService.UpdateUserAsync(user);
                if (!success)
                {
                    return BadRequest(new { message = "Failed to update user profile." });
                }

                return Ok(new { message = "Profile updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user profile for ID: {UserId}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            try
            {
                var success = await _userService.DeleteUserAsync(id);
                if (!success)
                {
                    return BadRequest(new { message = "Failed to delete user." });
                }

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
