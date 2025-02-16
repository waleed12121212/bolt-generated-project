using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Identity.Enums;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Models.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.Codecrete.QrCodeGenerator;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Bazingo_Application.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<string>> RegisterAsync(IRegisterModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserType = UserType.Customer,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return ApiResponse<string>.CreateError(result.Errors.Select(e => e.Description).ToList());
                }

                var token = await _tokenService.GenerateAccessTokenAsync(user.Id);
                return ApiResponse<string>.CreateSuccess(token, "Registration successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return ApiResponse<string>.CreateError("An error occurred during registration");
            }
        }

        public async Task<ApiResponse<string>> LoginAsync(ILoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return ApiResponse<string>.CreateError("Invalid credentials");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (!result.Succeeded)
                {
                    if (result.RequiresTwoFactor)
                    {
                        return ApiResponse<string>.CreateError("Two-factor authentication required");
                    }
                    if (result.IsLockedOut)
                    {
                        return ApiResponse<string>.CreateError("Account is locked out");
                    }
                    return ApiResponse<string>.CreateError("Invalid credentials");
                }

                var token = await _tokenService.GenerateAccessTokenAsync(user.Id);
                return ApiResponse<string>.CreateSuccess(token, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return ApiResponse<string>.CreateError("An error occurred during login");
            }
        }

        public async Task<ApiResponse<string>> ExternalLoginAsync(IExternalAuthModel model)
        {
            try
            {
                GoogleJsonWebSignature.Payload payload = null;
                if (model.Provider.ToLower() == "google")
                {
                    var settings = new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                    };
                    payload = await GoogleJsonWebSignature.ValidateAsync(model.Token, settings);
                    model.Email = payload.Email;
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        UserType = UserType.Customer,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        return ApiResponse<string>.CreateError("External login registration failed");
                    }
                }

                var token = await _tokenService.GenerateAccessTokenAsync(user.Id);
                return ApiResponse<string>.CreateSuccess(token, "External login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during external login");
                return ApiResponse<string>.CreateError("An error occurred during external login");
            }
        }

        public async Task<ApiResponse<bool>> Enable2FAAsync(string userId, bool enable)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.CreateError("User not found");
                }

                if (enable)
                {
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var unformattedKey = token.Replace(" ", "");

                    var qr = QrCode.EncodeText(
                        $"otpauth://totp/Bazingo:{user.Email}?secret={unformattedKey}&issuer=Bazingo",
                        QrCode.Ecc.Medium
                    );

                    var scale = 10;
                    var border = 4;
                    var size = (qr.Size + border * 2) * scale;

                    using var image = new Image<Rgba32>(size, size);
                    for (int y = 0; y < qr.Size; y++)
                    {
                        for (int x = 0; x < qr.Size; x++)
                        {
                            var color = qr.GetModule(x, y) ? new Rgba32(0, 0, 0) : new Rgba32(255, 255, 255);
                            for (int i = 0; i < scale; i++)
                            {
                                for (int j = 0; j < scale; j++)
                                {
                                    image[(x + border) * scale + i, (y + border) * scale + j] = color;
                                }
                            }
                        }
                    }

                    using var memoryStream = new MemoryStream();
                    await image.SaveAsPngAsync(memoryStream);
                    var qrCodeBase64 = Convert.ToBase64String(memoryStream.ToArray());

                    await _userManager.SetTwoFactorEnabledAsync(user, true);

                    var response = ApiResponse<bool>.CreateSuccess(true, "2FA setup initiated");
                    response.AdditionalData = new Dictionary<string, object>
                    {
                        { "qrCode", qrCodeBase64 }
                    };
                    return response;
                }
                else
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, false);
                    return ApiResponse<bool>.CreateSuccess(true, "2FA disabled");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during 2FA setup");
                return ApiResponse<bool>.CreateError("An error occurred during 2FA setup");
            }
        }

        public async Task<ApiResponse<string>> Verify2FAAsync(ITwoFactorModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return ApiResponse<string>.CreateError("User not found");
                }

                var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", model.Code);
                if (!isValid)
                {
                    return ApiResponse<string>.CreateError("Invalid verification code");
                }

                var token = await _tokenService.GenerateAccessTokenAsync(user.Id);
                return ApiResponse<string>.CreateSuccess(token, "2FA verification successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during 2FA verification");
                return ApiResponse<string>.CreateError("An error occurred during 2FA verification");
            }
        }

        public async Task<ApiResponse<bool>> Disable2FAAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse<bool>.CreateError("User not found");
                }

                var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
                if (!result.Succeeded)
                {
                    return ApiResponse<bool>.CreateError("Failed to disable 2FA");
                }

                return ApiResponse<bool>.CreateSuccess(true, "2FA disabled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during 2FA disabling");
                return ApiResponse<bool>.CreateError("An error occurred during 2FA disabling");
            }
        }

        public async Task<ApiResponse<string>> RefreshTokenAsync(string token)
        {
            try
            {
                var userId = await _tokenService.GetUserIdFromTokenAsync(token);
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResponse<string>.CreateError("Invalid token");
                }

                var newToken = await _tokenService.GenerateAccessTokenAsync(userId);
                return ApiResponse<string>.CreateSuccess(newToken, "Token refreshed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return ApiResponse<string>.CreateError("An error occurred during token refresh");
            }
        }

        public async Task<ApiResponse<bool>> RevokeTokenAsync(string token)
        {
            try
            {
                var result = await _tokenService.RevokeTokenAsync(token);
                return ApiResponse<bool>.CreateSuccess(result, result ? "Token revoked successfully" : "Failed to revoke token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token revocation");
                return ApiResponse<bool>.CreateError("An error occurred during token revocation");
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
    }
}
