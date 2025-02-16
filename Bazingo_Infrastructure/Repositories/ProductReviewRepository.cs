using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bazingo_Infrastructure.Repositories
{
    public class ProductReviewRepository : BaseRepository<ProductReviewEntity>, IProductReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ProductReviewEntity> GetByIdAsync(int id)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public new async Task<IEnumerable<ProductReviewEntity>> GetAllAsync()
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .Where(r => !r.IsDeleted)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetByProductIdAsync(int productId)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.User)
                    .Where(r => r.ProductId == productId && !r.IsDeleted)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetByUserIdAsync(string userId)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Where(r => r.UserId == userId && !r.IsDeleted)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public override async Task<ProductReviewEntity> AddAsync(ProductReviewEntity review)
        {
            try
            {
                // Check if user has already reviewed this product
                var existingReview = await _context.ProductReviews
                    .FirstOrDefaultAsync(r => r.ProductId == review.ProductId && 
                                            r.UserId == review.UserId && 
                                            !r.IsDeleted);

                if (existingReview != null)
                    return null;

                review.ReviewDate = DateTime.UtcNow;
                await _context.ProductReviews.AddAsync(review);
                return review;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override async Task<bool> UpdateAsync(ProductReviewEntity review)
        {
            try
            {
                var existingReview = await _context.ProductReviews
                    .FirstOrDefaultAsync(r => r.Id == review.Id && !r.IsDeleted);

                if (existingReview == null)
                    return false;

                existingReview.Rating = review.Rating;
                existingReview.Title = review.Title;
                existingReview.Comment = review.Comment;
                existingReview.LastUpdated = DateTime.UtcNow;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<bool> DeleteAsync(ProductReviewEntity review)
        {
            try
            {
                if (review == null)
                    return false;

                review.IsDeleted = true;
                review.LastUpdated = DateTime.UtcNow;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var review = await _context.ProductReviews.FindAsync(id);
                if (review == null)
                    return false;

                review.IsDeleted = true;
                review.LastUpdated = DateTime.UtcNow;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<bool> AnyAsync(Expression<Func<ProductReviewEntity, bool>> predicate)
        {
            try
            {
                return await _context.ProductReviews.AnyAsync(predicate);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<int> CountAsync(Expression<Func<ProductReviewEntity, bool>> predicate)
        {
            try
            {
                return await _context.ProductReviews.CountAsync(predicate);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override IQueryable<ProductReviewEntity> GetQueryable()
        {
            return _context.ProductReviews.AsQueryable();
        }

        public override async Task<IReadOnlyList<ProductReviewEntity>> FindAsync(Expression<Func<ProductReviewEntity, bool>> predicate)
        {
            try
            {
                return await _context.ProductReviews.Where(predicate).ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            try
            {
                return await _context.ProductReviews
                    .Where(r => r.ProductId == productId && !r.IsDeleted)
                    .AverageAsync(r => r.Rating);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> GetReviewCountAsync(int productId)
        {
            try
            {
                return await _context.ProductReviews
                    .CountAsync(r => r.ProductId == productId && !r.IsDeleted);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> HasUserReviewedAsync(int productId, string userId)
        {
            try
            {
                return await _context.ProductReviews
                    .AnyAsync(r => r.ProductId == productId && 
                                 r.UserId == userId && 
                                 !r.IsDeleted);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetVerifiedReviewsAsync(int productId)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.User)
                    .Where(r => r.ProductId == productId && 
                               r.IsVerifiedPurchase && 
                               !r.IsDeleted)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetRecentReviewsAsync(int count)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .Where(r => !r.IsDeleted)
                    .OrderByDescending(r => r.ReviewDate)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetTopRatedReviewsAsync(int count)
        {
            try
            {
                return await _context.ProductReviews
                    .Include(r => r.Product)
                    .Include(r => r.User)
                    .Where(r => !r.IsDeleted)
                    .OrderByDescending(r => r.Rating)
                    .ThenByDescending(r => r.Helpful)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<ProductReviewEntity>();
            }
        }

        public async Task<bool> MarkAsVerifiedPurchaseAsync(int reviewId)
        {
            try
            {
                var review = await _context.ProductReviews.FindAsync(reviewId);
                if (review == null)
                    return false;

                review.IsVerifiedPurchase = true;
                review.LastUpdated = DateTime.UtcNow;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
