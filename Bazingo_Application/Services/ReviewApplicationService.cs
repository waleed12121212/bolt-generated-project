using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bazingo_Application.Services
{
    public class ReviewApplicationService : IReviewService
    {
        private readonly IReviewService _reviewCoreService;
        private readonly ILogger<ReviewApplicationService> _logger;

        public ReviewApplicationService(IReviewService reviewCoreService, ILogger<ReviewApplicationService> logger)
        {
            _reviewCoreService = reviewCoreService ?? throw new ArgumentNullException(nameof(reviewCoreService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductReviewEntity> GetReviewByIdAsync(int reviewId)
        {
            return await _reviewCoreService.GetReviewByIdAsync(reviewId);
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByProductIdAsync(int productId)
        {
            return await _reviewCoreService.GetReviewsByProductIdAsync(productId);
        }

        public async Task<IEnumerable<ProductReviewEntity>> GetReviewsByUserIdAsync(string userId)
        {
            return await _reviewCoreService.GetReviewsByUserIdAsync(userId);
        }

        public async Task<ProductReviewEntity> CreateReviewAsync(ProductReviewEntity review)
        {
            return await _reviewCoreService.CreateReviewAsync(review);
        }

        public async Task<ProductReviewEntity> UpdateReviewAsync(ProductReviewEntity review)
        {
            return await _reviewCoreService.UpdateReviewAsync(review);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            return await _reviewCoreService.DeleteReviewAsync(reviewId);
        }
    }
}
