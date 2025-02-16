using Bazingo_Core.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<bool> IsEmailUniqueAsync(string email, string excludeUserId = null);
        Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null);
        Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
    }
}
