using System.Threading.Tasks;
using Bazingo_Core.Common;

namespace Bazingo_Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(string userId);
        Task<string> GenerateRefreshTokenAsync();
        Task<bool> ValidateTokenAsync(string token);
        Task<string> GetUserIdFromTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
