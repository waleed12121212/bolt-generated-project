using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Bazingo_Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, string excludeUserId = null)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null || (excludeUserId != null && user.Id == excludeUserId);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user == null || (excludeUserId != null && user.Id == excludeUserId);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            return await _userRepository.GetUsersByRoleAsync(role);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null)
                return false;

            return await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
