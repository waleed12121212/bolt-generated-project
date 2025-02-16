using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Bazingo_Application.DTOs.ProductReview;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Bazingo_Application.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductReviewService> _logger;

        public ProductReviewService(
            IProductReviewRepository reviewRepository,
            IProductRepository productRepository,
            ILogger<ProductReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<ProductReviewDto>> GetReviewByIdAsync(int id)
        {
            try
            {
                var review = await _reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    return ApiResponse<ProductReviewDto>.CreateError("Review not found");
                }

                var reviewDto = review.Adapt<ProductReviewDto>();
                return ApiResponse<ProductReviewDto>.CreateSuccess(reviewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review by id {Id}", id);
                return ApiResponse<ProductReviewDto>.CreateError("Error retrieving review");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetAllReviewsAsync()
        {
            try
            {
                var reviews = await _reviewRepository.GetAllAsync();
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all reviews");
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving reviews");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetProductReviewsAsync(int productId)
        {
            try
            {
                var reviews = await _reviewRepository.GetByProductIdAsync(productId);
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for product {ProductId}", productId);
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving product reviews");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetUserReviewsAsync(string userId)
        {
            try
            {
                var reviews = await _reviewRepository.GetByUserIdAsync(userId);
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reviews for user {UserId}", userId);
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving user reviews");
            }
        }

        public async Task<ApiResponse<ProductReviewSummaryDto>> GetProductReviewSummaryAsync(int productId)
        {
            try
            {
                var reviews = await _reviewRepository.GetByProductIdAsync(productId);
                var averageRating = await _reviewRepository.GetAverageRatingAsync(productId);
                var reviewCount = await _reviewRepository.GetReviewCountAsync(productId);

                var summary = new ProductReviewSummaryDto
                {
                    ProductId = productId,
                    AverageRating = averageRating,
                    TotalReviews = reviewCount,
                    VerifiedPurchases = reviews.Count(r => r.IsVerifiedPurchase),
                    RatingDistribution = reviews.GroupBy(r => r.Rating)
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                return ApiResponse<ProductReviewSummaryDto>.CreateSuccess(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review summary for product {ProductId}", productId);
                return ApiResponse<ProductReviewSummaryDto>.CreateError("Error retrieving review summary");
            }
        }

        public async Task<ApiResponse<ProductReviewDto>> CreateReviewAsync(CreateProductReviewDto dto, string userId)
        {
            try
            {
                var review = dto.Adapt<ProductReviewEntity>();
                review.UserId = userId;
                review.CreatedAt = DateTime.UtcNow;
                review.LastUpdated = DateTime.UtcNow;

                var createdReview = await _reviewRepository.AddAsync(review);
                if (createdReview == null)
                {
                    return ApiResponse<ProductReviewDto>.CreateError("Failed to create review");
                }

                var reviewDto = createdReview.Adapt<ProductReviewDto>();
                return ApiResponse<ProductReviewDto>.CreateSuccess(reviewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for product {ProductId}", dto.ProductId);
                return ApiResponse<ProductReviewDto>.CreateError("Error creating review");
            }
        }

        public async Task<ApiResponse<ProductReviewDto>> UpdateReviewAsync(int id, UpdateProductReviewDto dto, string userId)
        {
            try
            {
                var existingReview = await _reviewRepository.GetByIdAsync(id);
                if (existingReview == null)
                {
                    return ApiResponse<ProductReviewDto>.CreateError("Review not found");
                }

                if (existingReview.UserId != userId)
                {
                    return ApiResponse<ProductReviewDto>.CreateError("Unauthorized to update this review");
                }

                // Update properties
                existingReview.Rating = dto.Rating;
                existingReview.Title = dto.Title;
                existingReview.Comment = dto.Comment;
                existingReview.LastUpdated = DateTime.UtcNow;

                var success = await _reviewRepository.UpdateAsync(existingReview);
                if (!success)
                {
                    return ApiResponse<ProductReviewDto>.CreateError("Failed to update review");
                }

                var reviewDto = existingReview.Adapt<ProductReviewDto>();
                return ApiResponse<ProductReviewDto>.CreateSuccess(reviewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review {Id}", id);
                return ApiResponse<ProductReviewDto>.CreateError("Error updating review");
            }
        }

        public async Task<ApiResponse<bool>> DeleteReviewAsync(int id, string userId)
        {
            try
            {
                var review = await _reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    return ApiResponse<bool>.CreateError("Review not found");
                }

                if (review.UserId != userId)
                {
                    return ApiResponse<bool>.CreateError("Unauthorized to delete this review");
                }

                var result = await _reviewRepository.DeleteAsync(id);
                return result 
                    ? ApiResponse<bool>.CreateSuccess(true)
                    : ApiResponse<bool>.CreateError("Failed to delete review");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review {Id}", id);
                return ApiResponse<bool>.CreateError("Error deleting review");
            }
        }

        public async Task<ApiResponse<double>> GetAverageRatingAsync(int productId)
        {
            try
            {
                var rating = await _reviewRepository.GetAverageRatingAsync(productId);
                return ApiResponse<double>.CreateSuccess(rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting average rating for product {ProductId}", productId);
                return ApiResponse<double>.CreateError("Error retrieving average rating");
            }
        }

        public async Task<ApiResponse<int>> GetReviewCountAsync(int productId)
        {
            try
            {
                var count = await _reviewRepository.GetReviewCountAsync(productId);
                return ApiResponse<int>.CreateSuccess(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review count for product {ProductId}", productId);
                return ApiResponse<int>.CreateError("Error retrieving review count");
            }
        }

        public async Task<ApiResponse<bool>> HasUserReviewedAsync(int productId, string userId)
        {
            try
            {
                var hasReviewed = await _reviewRepository.HasUserReviewedAsync(productId, userId);
                return ApiResponse<bool>.CreateSuccess(hasReviewed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} has reviewed product {ProductId}", userId, productId);
                return ApiResponse<bool>.CreateError("Error checking review status");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetRecentReviewsAsync(int count)
        {
            try
            {
                var reviews = await _reviewRepository.GetRecentReviewsAsync(count);
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent reviews");
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving recent reviews");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetTopRatedReviewsAsync(int count)
        {
            try
            {
                var reviews = await _reviewRepository.GetTopRatedReviewsAsync(count);
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top rated reviews");
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving top rated reviews");
            }
        }

        public async Task<ApiResponse<List<ProductReviewDto>>> GetVerifiedReviewsAsync(int productId)
        {
            try
            {
                var reviews = await _reviewRepository.GetVerifiedReviewsAsync(productId);
                var reviewDtos = reviews.Adapt<List<ProductReviewDto>>();
                return ApiResponse<List<ProductReviewDto>>.CreateSuccess(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting verified reviews for product {ProductId}", productId);
                return ApiResponse<List<ProductReviewDto>>.CreateError("Error retrieving verified reviews");
            }
        }

        public async Task<ApiResponse<bool>> MarkAsVerifiedPurchaseAsync(int reviewId)
        {
            try
            {
                var result = await _reviewRepository.MarkAsVerifiedPurchaseAsync(reviewId);
                return result 
                    ? ApiResponse<bool>.CreateSuccess(true)
                    : ApiResponse<bool>.CreateError("Failed to mark review as verified");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking review {ReviewId} as verified purchase", reviewId);
                return ApiResponse<bool>.CreateError("Error marking review as verified");
            }
        }
    }
}
