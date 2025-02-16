    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Bazingo_Core.Entities.Product;

    namespace Bazingo_Core.Interfaces
    {
        public interface IReviewService
        {
            Task<ProductReviewEntity> GetReviewByIdAsync(int reviewId);
            Task<IEnumerable<ProductReviewEntity>> GetReviewsByProductIdAsync(int productId);
            Task<IEnumerable<ProductReviewEntity>> GetReviewsByUserIdAsync(string userId);
            Task<ProductReviewEntity> CreateReviewAsync(ProductReviewEntity review);
            Task<ProductReviewEntity> UpdateReviewAsync(ProductReviewEntity review);
            Task<bool> DeleteReviewAsync(int reviewId);
        }
    }
