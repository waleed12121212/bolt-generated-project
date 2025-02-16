using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Bazingo_Application.DTOs;
using Bazingo_Core.Entities.Identity;
using System.Linq;
using Bazingo_Application.Interfaces;
using Bazingo_Application.DTOs.Users;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserApplicationService _userService;

    public UserController(IUserApplicationService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userService.GetUserByEmailAsync(userLoginDTO.Email);
        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        // Note: In a real application, you would validate the password here
        // and generate a JWT token for authentication

        return Ok(new { message = "User logged in successfully." });
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetUserProfile(string id)
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

    [HttpPut("profile/{id}")]
    public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UpdateUserProfileDTO updateProfileDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return BadRequest(new { message = "Failed to delete user." });
        }

        return Ok(new { message = "User deleted successfully." });
    }
}
