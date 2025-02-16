    using Bazingo_Application.Models;
    using Bazingo_Core.Configuration;
    using Bazingo_Core.Entities.Identity;
    using Bazingo_Core.Interfaces;
    using Bazingo_Core.Models.Identity;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Bazingo_Core;

    namespace Bazingo_Application.Services.Identity
    {
        public class AuthService : IAuthService
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly JwtSettings _jwtSettings;
            private readonly ILogger<AuthService> _logger;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly IEmailService _emailService;

            public AuthService(
                UserManager<ApplicationUser> userManager,
                IOptions<JwtSettings> jwtSettings,
                ILogger<AuthService> logger,
                SignInManager<ApplicationUser> signInManager,
                IEmailService emailService)
            {
                _userManager = userManager;
                _jwtSettings = jwtSettings.Value;
                _logger = logger;
                _signInManager = signInManager;
                _emailService = emailService;
            }

            public async Task<ApiResponse<string>> RegisterAsync(IRegisterModel model)
            {
                try
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        return ApiResponse<string>.CreateError("Email is already registered");
                    }

                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        return ApiResponse<string>.CreateError(result.Errors.Select(e => e.Description).ToList());
                    }

                    var token = await GenerateJwtToken(user);
                    return ApiResponse<string>.CreateSuccess(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during registration");
                    return ApiResponse<string>.CreateError("Registration failed");
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

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (!result.Succeeded)
                    {
                        return ApiResponse<string>.CreateError("Invalid credentials");
                    }

                    var token = await GenerateJwtToken(user);
                    return ApiResponse<string>.CreateSuccess(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during login");
                    return ApiResponse<string>.CreateError("Login failed");
                }
            }

            public async Task<ApiResponse<string>> ExternalLoginAsync(IExternalAuthModel model)
            {
                try
                {
                    // Placeholder for external authentication logic
                    // In a real application, you would integrate with external providers like Google, Facebook, etc.
                    // For this example, we'll just create a dummy user and generate a token.

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            FirstName = "External",
                            LastName = "User",
                            PhoneNumber = "123-456-7890"
                        };

                        var result = await _userManager.CreateAsync(user);
                        if (!result.Succeeded)
                        {
                            return ApiResponse<string>.CreateError("External login failed");
                        }
                    }

                    var token = await GenerateJwtToken(user);
                    return ApiResponse<string>.CreateSuccess(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during external login");
                    return ApiResponse<string>.CreateError("External login failed");
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

                    // Placeholder for 2FA enabling logic
                    // In a real application, you would generate a 2FA secret and store it for the user.

                    return ApiResponse<bool>.CreateSuccess(true, "2FA enabling logic not implemented in this example.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error enabling 2FA");
                    return ApiResponse<bool>.CreateError("Failed to enable 2FA");
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

                    // Placeholder for 2FA verification logic
                    // In a real application, you would verify the user's 2FA code against the stored secret.

                    return ApiResponse<string>.CreateSuccess("2FA verification logic not implemented in this example.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error verifying 2FA");
                    return ApiResponse<string>.CreateError("2FA verification failed");
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

                    // Placeholder for 2FA disabling logic
                    // In a real application, you would remove the stored 2FA secret for the user.

                    return ApiResponse<bool>.CreateSuccess(true, "2FA disabling logic not implemented in this example.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disabling 2FA");
                    return ApiResponse<bool>.CreateError("Failed to disable 2FA");
                }
            }

            public async Task<ApiResponse<string>> RefreshTokenAsync(string token)
            {
                try
                {
                    // Placeholder for refresh token logic
                    // In a real application, you would validate the refresh token and generate a new access token.

                    return ApiResponse<string>.CreateSuccess("Refresh token logic not implemented in this example.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error refreshing token");
                    return ApiResponse<string>.CreateError("Token refresh failed");
                }
            }

            public async Task<ApiResponse<bool>> RevokeTokenAsync(string token)
            {
                try
                {
                    // Placeholder for token revocation logic
                    // In a real application, you would invalidate the token in a database or cache.

                    return ApiResponse<bool>.CreateSuccess(true, "Token revocation logic not implemented in this example.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error revoking token");
                    return ApiResponse<bool>.CreateError("Token revocation failed");
                }
            }

            public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) return false;
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
                if (user == null) return false;
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }

            public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                return result.Succeeded;
            }

            public async Task<string> GeneratePasswordResetTokenAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }

            public async Task<bool> ResetPasswordAsync(string userId, string token, string newPassword)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                return result.Succeeded;
            }

            public async Task<bool> IsEmailConfirmedAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;
                return await _userManager.IsEmailConfirmedAsync(user);
            }

            public async Task<bool> ConfirmEmailAsync(string userId, string token)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return result.Succeeded;
            }

            private async Task<string> GenerateJwtToken(ApplicationUser user)
            {
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var roles = await _userManager.GetRolesAsync(user);
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtSettings.ValidIssuer,
                    Audience = _jwtSettings.ValidAudience
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    ValidAudience = _jwtSettings.ValidAudience,
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
        }
    }
