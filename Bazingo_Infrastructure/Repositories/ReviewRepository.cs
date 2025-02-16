    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Linq.Expressions;
    using Bazingo_Core.Entities.Product;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class ReviewRepository : BaseRepository<ProductReviewEntity>, IProductReviewRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly DbSet<ProductReviewEntity> _dbSet;
            private readonly ILogger<ReviewRepository> _logger;

            public ReviewRepository(ApplicationDbContext context, ILogger<ReviewRepository> logger) : base(context)
            {
                _context = context;
                _dbSet = context.Set<ProductReviewEntity>();
                _logger = logger;
            }

            public new async Task<ProductReviewEntity> GetByIdAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting review by ID: {ReviewId}", id);
                    return null;
                }
            }

            public new async Task<IEnumerable<ProductReviewEntity>> GetAllAsync()
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => !r.IsDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all reviews");
                    return new List<ProductReviewEntity>();
                }
            }

            public new async Task<ProductReviewEntity> AddAsync(ProductReviewEntity review)
            {
                try
                {
                    review.ReviewDate = DateTime.UtcNow;
                    review.CreatedAt = DateTime.UtcNow;
                    await _dbSet.AddAsync(review);
                    await _context.SaveChangesAsync();
                    return review;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding review for product {ProductId} by user {UserId}", review.ProductId, review.UserId);
                    return null;
                }
            }

            public async Task<bool> UpdateAsync(ProductReviewEntity review)
            {
                try
                {
                    review.LastUpdated = DateTime.UtcNow;
                    _context.Entry(review).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating review {ReviewId}", review.Id);
                    return false;
                }
            }

            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    var review = await GetByIdAsync(id);
                    if (review != null)
                    {
                        review.IsDeleted = true;
                        review.LastUpdated = DateTime.UtcNow;
                        _context.Entry(review).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting review {ReviewId}", id);
                    return false;
                }
            }

            public async Task<IEnumerable<ProductReviewEntity>> GetByProductIdAsync(int productId)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => r.ProductId == productId && !r.IsDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting reviews for product {ProductId}", productId);
                    return new List<ProductReviewEntity>();
                }
            }

            public async Task<IEnumerable<ProductReviewEntity>> GetByUserIdAsync(string userId)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => r.UserId == userId && !r.IsDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting reviews for user {UserId}", userId);
                    return new List<ProductReviewEntity>();
                }
            }

            public async Task<double> GetAverageRatingAsync(int productId)
            {
                try
                {
                    return await _dbSet
                        .Where(r => r.ProductId == productId && !r.IsDeleted)
                        .AverageAsync(r => r.Rating);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting average rating for product {ProductId}", productId);
                    return 0;
                }
            }

            public async Task<int> GetReviewCountAsync(int productId)
            {
                try
                {
                    return await _dbSet
                        .CountAsync(r => r.ProductId == productId && !r.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting review count for product {ProductId}", productId);
                    return 0;
                }
            }

            public async Task<bool> HasUserReviewedAsync(int productId, string userId)
            {
                try
                {
                    return await _dbSet
                        .AnyAsync(r => r.ProductId == productId && r.UserId == userId && !r.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if user {UserId} has reviewed product {ProductId}", userId, productId);
                    return false;
                }
            }

            public async Task<IEnumerable<ProductReviewEntity>> GetRecentReviewsAsync(int count)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => !r.IsDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .Take(count)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting recent reviews");
                    return new List<ProductReviewEntity>();
                }
            }

            public async Task<IEnumerable<ProductReviewEntity>> GetTopRatedReviewsAsync(int count)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => !r.IsDeleted)
                        .OrderByDescending(r => r.Rating)
                        .ThenByDescending(r => r.Helpful)
                        .Take(count)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting top rated reviews");
                    return new List<ProductReviewEntity>();
                }
            }

            public async Task<IEnumerable<ProductReviewEntity>> GetVerifiedReviewsAsync(int productId)
            {
                try
                {
                    return await _dbSet
                        .Include(r => r.User)
                        .Include(r => r.Product)
                        .Where(r => r.ProductId == productId && r.IsVerifiedPurchase && !r.IsDeleted)
                        .OrderByDescending(r => r.ReviewDate)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting verified reviews for product {ProductId}", productId);
                    return new List<ProductReviewEntity>();
                }
            }

            public async Task<bool> MarkAsVerifiedPurchaseAsync(int reviewId)
            {
                try
                {
                    var review = await GetByIdAsync(reviewId);
                    if (review != null)
                    {
                        review.IsVerifiedPurchase = true;
                        review.LastUpdated = DateTime.UtcNow;
                        return await UpdateAsync(review);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error marking review {ReviewId} as verified purchase", reviewId);
                    return false;
                }
            }
        }
    }
