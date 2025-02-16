using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bazingo_Core.Services
{
    public class ReviewCoreService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewCoreService> _logger;

        public ReviewCoreService(IUnitOfWork unitOfWork, ILogger<ReviewCoreService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductReviewEntity> GetReviewByIdAsync(int reviewId)
        {
            return await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByProductIdAsync(int productId)
        {
            return await _unitOfWork.ProductReviews.GetByProductIdAsync(productId);
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByUserIdAsync(string userId)
        {
            return await _unitOfWork.ProductReviews.GetByUserIdAsync(userId);
        }

        public async Task<ProductReviewEntity> CreateReviewAsync(ProductReviewEntity review)
        {
            await _unitOfWork.ProductReviews.AddAsync(review);
            await _unitOfWork.CompleteAsync();
            return review;
        }

        public async Task<ProductReviewEntity> UpdateReviewAsync(ProductReviewEntity review)
        {
            var success = await _unitOfWork.ProductReviews.UpdateAsync(review);
            if (success)
            {
                await _unitOfWork.CompleteAsync();
            }
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var success = await _unitOfWork.ProductReviews.DeleteAsync(reviewId);
            if (success)
            {
                await _unitOfWork.CompleteAsync();
            }
            return success;
        }
    }
}
