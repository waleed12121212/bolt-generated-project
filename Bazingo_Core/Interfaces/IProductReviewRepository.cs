using Bazingo_Core.Entities.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface IProductReviewRepository : IBaseRepository<ProductReviewEntity>
    {
        Task<ProductReviewEntity> GetByIdAsync(int id);
        Task<IEnumerable<ProductReviewEntity>> GetAllAsync();
        Task<IEnumerable<ProductReviewEntity>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductReviewEntity>> GetByUserIdAsync(string userId);
        Task<ProductReviewEntity> AddAsync(ProductReviewEntity review);
        Task<bool> UpdateAsync(ProductReviewEntity review);
        Task<bool> DeleteAsync(int id);
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetReviewCountAsync(int productId);
        Task<bool> HasUserReviewedAsync(int productId, string userId);
        Task<IEnumerable<ProductReviewEntity>> GetRecentReviewsAsync(int count);
        Task<IEnumerable<ProductReviewEntity>> GetTopRatedReviewsAsync(int count);
        Task<IEnumerable<ProductReviewEntity>> GetVerifiedReviewsAsync(int productId);
        Task<bool> MarkAsVerifiedPurchaseAsync(int reviewId);
    }
}
