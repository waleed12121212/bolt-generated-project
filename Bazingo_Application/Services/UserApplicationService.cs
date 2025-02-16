using Bazingo_Core.Entities.Identity;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Application.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly Bazingo_Core.Interfaces.IUserService _userService;
        private readonly ILogger<UserApplicationService> _logger;

        public UserApplicationService(Bazingo_Core.Interfaces.IUserService userService, ILogger<UserApplicationService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _userService.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID {UserId}", userId);
                throw;
            }
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userService.GetUserByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userService.GetUserByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username {Username}", username);
                throw;
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email, string excludeUserId = null)
        {
            try
            {
                return await _userService.IsEmailUniqueAsync(email, excludeUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email uniqueness for {Email}", email);
                throw;
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null)
        {
            try
            {
                return await _userService.IsUsernameUniqueAsync(username, excludeUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username uniqueness for {Username}", username);
                throw;
            }
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            try
            {
                return await _userService.GetUsersByRoleAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by role {Role}", role);
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                return await _userService.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", user.Id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                return await _userService.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                throw;
            }
        }
    }
}
