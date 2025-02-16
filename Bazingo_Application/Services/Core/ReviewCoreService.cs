using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Product;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Application.Services.Core
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
            try
            {
                return await _unitOfWork.ProductReviews.GetByIdAsync(reviewId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review by ID {ReviewId}", reviewId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByProductIdAsync(int productId)
        {
            try
            {
                return await _unitOfWork.ProductReviews.GetByProductIdAsync(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for product {ProductId}", productId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByUserIdAsync(string userId)
        {
            try
            {
                return await _unitOfWork.ProductReviews.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for user {UserId}", userId);
                throw;
            }
        }

        public async Task<ProductReviewEntity> CreateReviewAsync(ProductReviewEntity review)
        {
            try
            {
                await _unitOfWork.ProductReviews.AddAsync(review);
                await _unitOfWork.CompleteAsync();
                return review;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for product {ProductId} by user {UserId}", review.ProductId, review.UserId);
                throw;
            }
        }

        public async Task<ProductReviewEntity> UpdateReviewAsync(ProductReviewEntity review)
        {
            try
            {
                var success = await _unitOfWork.ProductReviews.UpdateAsync(review);
                if (!success)
                {
                    throw new InvalidOperationException($"Failed to update review with ID {review.Id}");
                }
                await _unitOfWork.CompleteAsync();
                return review;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review {ReviewId}", review.Id);
                throw;
            }
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            try
            {
                var success = await _unitOfWork.ProductReviews.DeleteAsync(reviewId);
                if (success)
                {
                    await _unitOfWork.CompleteAsync();
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {ReviewId}", reviewId);
                throw;
            }
        }
    }
}
