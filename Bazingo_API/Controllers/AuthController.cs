    using Bazingo_API.Controllers;
    using Bazingo_Application.DTOs.Auth;
    using Bazingo_Application.DTOs.Users;
    using Bazingo_Core;
    using Bazingo_Core.Interfaces;
    using Bazingo_Core.Models.Common;
    using Bazingo_Core.Models.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Models;
    using Bazingo_Application.DTOs.Auth; // New using directive

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;
            private readonly ILogger<AuthController> _logger;

            public AuthController(IAuthService authService , ILogger<AuthController> logger)
            {
                _authService = authService;
                _logger = logger;
            }

            private ActionResult<Bazingo_Core.Models.Common.ApiResponse<T>> OkResponse<T>(T data , string message = null)
            {
                return Ok(new Bazingo_Core.Models.Common.ApiResponse<T>
                {
                    Succeeded = true ,
                    Message = message ?? "Operation successful" ,
                    Data = data
                });
            }

            private ActionResult<Bazingo_Core.Models.Common.ApiResponse<T>> ErrorResponse<T>(string message , int statusCode = 400)
            {
                return StatusCode(statusCode , new Bazingo_Core.Models.Common.ApiResponse<T>
                {
                    Succeeded = false ,
                    Message = message
                });
            }

            [HttpPost("register")]
            [AllowAnonymous]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<string>>> Register([FromBody] RegisterDto model)
            {
                try
                {
                    _logger.LogInformation("Registration attempt for user: {Email}" , model.Email);

                    if (!ModelState.IsValid)
                    {
                        return ErrorResponse<string>("Invalid registration data", 400);
                    }

                    var result = await _authService.RegisterAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User registered successfully: {Email}" , model.Email);
                        return OkResponse(result.Data , result.Message);
                    }

                    _logger.LogWarning("Registration failed for user: {Email}. Reason: {Message}" ,
                        model.Email , result.Message);
                    return ErrorResponse<string>(result.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error during registration for user: {Email}" , model.Email);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<string>>> Login([FromBody] LoginDto model)
            {
                try
                {
                    _logger.LogInformation("Login attempt for user: {Email}" , model.Email);

                    if (!ModelState.IsValid)
                    {
                        return ErrorResponse<string>("Invalid login data", 400);
                    }

                    var result = await _authService.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in successfully: {Email}" , model.Email);
                        return OkResponse(result.Data , result.Message);
                    }

                    _logger.LogWarning("Login failed for user: {Email}. Reason: {Message}" ,
                        model.Email , result.Message);
                    return ErrorResponse<string>(result.Message , 401);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error during login for user: {Email}" , model.Email);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("refresh-token")]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<string>>> RefreshToken([FromBody] RefreshTokenDto model)
            {
                try
                {
                    if (model == null || string.IsNullOrEmpty(model.RefreshToken))
                    {
                        return ErrorResponse<string>("Invalid refresh token", 400);
                    }

                    var result = await _authService.RefreshTokenAsync(model.RefreshToken);
                    if (result.Succeeded)
                    {
                        return OkResponse(result.Data , result.Message);
                    }

                    return ErrorResponse<string>(result.Message , 401);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error during token refresh");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("logout")]
            [Authorize]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<bool>>> Logout( )
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return ErrorResponse<bool>("User not authenticated", 401);
                    }

                    _logger.LogInformation("Logout attempt for user ID: {UserId}" , userId);

                    await _authService.RevokeTokenAsync(userId);
                    _logger.LogInformation("User logged out successfully: {UserId}" , userId);
                    return OkResponse(true , "Logged out successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error during logout");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("external-login")]
            [AllowAnonymous]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<string>>> ExternalLogin([FromBody] ExternalAuthDto model)
            {
                try
                {
                    if (model == null || string.IsNullOrEmpty(model.Provider) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Email))
                    {
                        return ErrorResponse<string>("Invalid external auth data", 400);
                    }

                    _logger.LogInformation("External login attempt for user: {Email} with provider: {Provider}" ,
                        model.Email , model.Provider);

                    var result = await _authService.ExternalLoginAsync(model);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("External login successful for user: {Email}" , model.Email);
                        return OkResponse(result.Data , result.Message);
                    }

                    _logger.LogWarning("External login failed for user: {Email}. Reason: {Message}" ,
                        model.Email , result.Message);
                    return ErrorResponse<string>(result.Message , 401);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error during external login for user: {Email}" , model.Email);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("2fa/enable")]
            [Authorize]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<bool>>> Enable2FA([FromBody] Enable2FADto model)
            {
                try
                {
                    var result = await _authService.Enable2FAAsync(User.Identity.Name , true);
                    if (result.Succeeded)
                    {
                        return OkResponse<bool>(true , "2FA setup successful. Please scan the QR code with your authenticator app.");
                    }
                    return ErrorResponse<bool>("Failed to enable 2FA");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error enabling 2FA for user: {UserId}" , User.Identity.Name);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("2fa/verify")]
            [Authorize]
            public async Task<ActionResult<Bazingo_Core.Models.Common.ApiResponse<bool>>> Verify2FA([FromBody] Verify2FADtoNew model)
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    if (string.IsNullOrEmpty(userId) || model == null || string.IsNullOrEmpty(model.Code))
                    {
                        return ErrorResponse<bool>("Invalid 2FA data", 400);
                    }

                    var twoFactorModel = new TwoFactorModel
                    {
                        UserId = userId ,
                        Code = model.Code ,
                        RememberDevice = model.RememberDevice
                    };
                    var result = await _authService.Verify2FAAsync(twoFactorModel);
                    if (result.Succeeded)
                    {
                        return OkResponse(true , "2FA verification successful");
                    }
                    return ErrorResponse<bool>("Invalid verification code" , 401);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex , "Error verifying 2FA for user: {UserId}" , model.UserId);
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
