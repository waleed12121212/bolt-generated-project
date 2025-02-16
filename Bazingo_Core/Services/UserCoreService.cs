using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Bazingo_Core.Services
{
    public class UserCoreService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserCoreService> _logger;

        public UserCoreService(UserManager<ApplicationUser> userManager, ILogger<UserCoreService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _userManager.FindByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID {UserId}", userId);
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for deletion {UserId}", userId);
                    return false;
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to delete user {UserId}. Errors: {Errors}", userId, errors);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", userId);
                throw;
            }
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllUsersAsync()
        {
            try
            {
                return await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _userManager.FindByNameAsync(username);
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
                var user = await _userManager.FindByEmailAsync(email);
                return user == null || (excludeUserId != null && user.Id == excludeUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email uniqueness {Email}", email);
                throw;
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                return user == null || (excludeUserId != null && user.Id == excludeUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username uniqueness {Username}", username);
                throw;
            }
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role)
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                return usersInRole.ToList();
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
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to update user {UserId}. Errors: {Errors}", user.Id, errors);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", user.Id);
                throw;
            }
        }
    }
}
