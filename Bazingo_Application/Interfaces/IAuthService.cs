using System.Threading.Tasks;
using Bazingo_Application.DTOs;
using Bazingo_Core.Models.Identity;

namespace Bazingo_Application.Interfaces
{
    public interface IAuthService
    {
        Task<Bazingo_Core.Models.Identity.AuthResult> RegisterAsync(Bazingo_Core.Models.Identity.RegisterModel model);
        Task<Bazingo_Core.Models.Identity.AuthResult> LoginAsync(Bazingo_Core.Models.Identity.LoginModel model);
        Task<Bazingo_Core.Models.Identity.AuthResult> ExternalLoginAsync(Bazingo_Core.Models.Identity.ExternalAuthModel model);
        Task<Bazingo_Core.Models.Identity.AuthResult> RefreshTokenAsync(string refreshToken);
        Task<Bazingo_Core.Models.Identity.AuthResult> Verify2FAAsync(Bazingo_Core.Models.Identity.ITwoFactorModel model);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> Enable2FAAsync(string userId);
        Task<bool> Disable2FAAsync(string userId);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> LogoutAsync(string userId);
    }
}
