    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Identity;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class UserRepository : IUserRepository
        {
            private readonly AppIdentityDbContext _context;
            private readonly ILogger<UserRepository> _logger;

            public UserRepository(AppIdentityDbContext context, ILogger<UserRepository> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<ApplicationUser> GetByIdAsync(string id)
            {
                try
                {
                    return await _context.Users.FindAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user by ID {UserId}", id);
                    return null;
                }
            }

            public async Task<ApplicationUser> GetByEmailAsync(string email)
            {
                try
                {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user by email {Email}", email);
                    return null;
                }
            }

            public async Task<ApplicationUser> GetByUsernameAsync(string username)
            {
                try
                {
                    return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user by username {Username}", username);
                    return null;
                }
            }

            public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync()
            {
                try
                {
                    return await _context.Users
                        .OrderBy(u => u.UserName)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all users");
                    return new List<ApplicationUser>();
                }
            }

            public async Task<bool> IsEmailUniqueAsync(string email, string excludeUserId = null)
            {
                try
                {
                    var query = _context.Users.Where(u => u.Email == email);
                    if (!string.IsNullOrEmpty(excludeUserId))
                    {
                        query = query.Where(u => u.Id != excludeUserId);
                    }
                    return !await query.AnyAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking email uniqueness {Email}", email);
                    return false;
                }
            }

            public async Task<bool> IsUsernameUniqueAsync(string username, string excludeUserId = null)
            {
                try
                {
                    var query = _context.Users.Where(u => u.UserName == username);
                    if (!string.IsNullOrEmpty(excludeUserId))
                    {
                        query = query.Where(u => u.Id != excludeUserId);
                    }
                    return !await query.AnyAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking username uniqueness {Username}", username);
                    return false;
                }
            }

            public async Task<IReadOnlyList<ApplicationUser>> GetUsersByRoleAsync(string role)
            {
                try
                {
                    var usersInRole = await _context.UserRoles
                        .Where(ur => ur.RoleId == role)
                        .Select(ur => ur.UserId)
                        .ToListAsync();

                    return await _context.Users
                        .Where(u => usersInRole.Contains(u.Id))
                        .OrderBy(u => u.UserName)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting users by role {Role}", role);
                    return new List<ApplicationUser>();
                }
            }

            public async Task<bool> UpdateUserAsync(ApplicationUser user)
            {
                try
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user {UserId}", user.Id);
                    return false;
                }
            }

            public async Task<bool> DeleteUserAsync(string userId)
            {
                try
                {
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                    {
                        _logger.LogWarning("User not found for deletion {UserId}", userId);
                        return false;
                    }

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting user {UserId}", userId);
                    return false;
                }
            }
        }
    }
