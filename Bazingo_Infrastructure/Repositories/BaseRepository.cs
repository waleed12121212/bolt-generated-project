    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class BaseRepository<T> : IBaseRepository<T> where T : class
        {
            protected readonly ApplicationDbContext _context;
            protected readonly DbSet<T> _dbSet;
            protected readonly ILogger<BaseRepository<T>> _logger;

            public BaseRepository(ApplicationDbContext context, ILogger<BaseRepository<T>> logger)
            {
                _context = context;
                _dbSet = context.Set<T>();
                _logger = logger;
            }

            public virtual async Task<T> GetByIdAsync(int id)
            {
                try
                {
                    return await _dbSet.FindAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting entity by ID: {EntityId}", id);
                    return null;
                }
            }

            public virtual async Task<IReadOnlyList<T>> GetAllAsync()
            {
                try
                {
                    return await _dbSet.ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all entities");
                    return new List<T>();
                }
            }

            public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
            {
                try
                {
                    return await _dbSet.Where(predicate).ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error finding entities with predicate");
                    return new List<T>();
                }
            }

            public virtual async Task<T> AddAsync(T entity)
            {
                try
                {
                    await _dbSet.AddAsync(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding entity");
                    return null;
                }
            }

            public virtual async Task UpdateAsync(T entity)
            {
                try
                {
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating entity");
                }
            }

            public virtual async Task DeleteAsync(T entity)
            {
                try
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting entity");
                }
            }

            public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            {
                try
                {
                    return await _dbSet.AnyAsync(predicate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if any entities exist with predicate");
                    return false;
                }
            }

            public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
            {
                try
                {
                    return await _dbSet.CountAsync(predicate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error counting entities with predicate");
                    return 0;
                }
            }

            public virtual IQueryable<T> GetQueryable()
            {
                return _dbSet.AsQueryable();
            }
        }
    }
