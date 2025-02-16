using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Review;

namespace Bazingo_Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<ProductReview> GetByIdAsync(int id);
        Task<IEnumerable<ProductReview>> GetAllAsync();
        Task<IEnumerable<ProductReview>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductReview>> GetByUserIdAsync(string userId);
        Task<ProductReview> AddAsync(ProductReview review);
        Task<bool> UpdateAsync(ProductReview review);
        Task<bool> DeleteAsync(int id);
        Task<double> GetAverageRatingAsync(int productId);
        Task<int> GetReviewCountAsync(int productId);
        Task<bool> HasUserReviewedAsync(int productId, string userId);
        Task<IEnumerable<ProductReview>> GetLatestReviewsAsync(int count);
    }
}
