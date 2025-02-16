using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByUsernameAsync(string username);
        Task<IReadOnlyList<ApplicationUser>> GetAllAsync();
        Task<bool> IsEmailUniqueAsync(string email, string excludeUserId = null);
        Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null);
        Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
    }
}
