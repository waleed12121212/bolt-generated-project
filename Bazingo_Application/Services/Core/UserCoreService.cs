    using Bazingo_Core.Entities.Identity;
    using Bazingo_Core.Interfaces;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace Bazingo_Application.Services.Core
    {
        public class UserCoreService : IUserService
        {
            private readonly IUserRepository _userRepository;
            private readonly ILogger<UserCoreService> _logger;

            public UserCoreService(IUserRepository userRepository, ILogger<UserCoreService> logger)
            {
                _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<ApplicationUser> GetUserByIdAsync(string userId)
            {
                try
                {
                    return await _userRepository.GetByIdAsync(userId);
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
                    return await _userRepository.GetByEmailAsync(email);
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
                    return await _userRepository.GetByUsernameAsync(username);
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
                    return await _userRepository.IsEmailUniqueAsync(email, excludeUserId);
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
                    return await _userRepository.IsUsernameUniqueAsync(username, excludeUserId);
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
                    return await _userRepository.GetUsersByRoleAsync(role);
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
                    return await _userRepository.UpdateUserAsync(user);
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
                    return await _userRepository.DeleteUserAsync(userId);
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
                    return await _userRepository.GetAllAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all users");
                    throw;
                }
            }
        }
    }
