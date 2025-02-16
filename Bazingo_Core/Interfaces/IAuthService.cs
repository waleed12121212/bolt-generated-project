using System.Threading.Tasks;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Models.Identity;

namespace Bazingo_Core.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(IRegisterModel model);
        Task<ApiResponse<string>> LoginAsync(ILoginModel model);
        Task<ApiResponse<string>> ExternalLoginAsync(IExternalAuthModel model);
        Task<ApiResponse<bool>> Enable2FAAsync(string userId, bool enable);
        Task<ApiResponse<string>> Verify2FAAsync(ITwoFactorModel model);
        Task<ApiResponse<bool>> Disable2FAAsync(string userId);
        Task<ApiResponse<string>> RefreshTokenAsync(string token);
        Task<ApiResponse<bool>> RevokeTokenAsync(string token);

        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<bool> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<bool> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}
