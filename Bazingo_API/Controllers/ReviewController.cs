    using Bazingo_Application.DTOs.Reviews;
    using Bazingo_Core.DomainLogic;
    using Bazingo_Core.Entities.Review;
    using Bazingo_Core.Entities.Product;
    using Bazingo_Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Mapster;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class ReviewController : ControllerBase
        {
            private readonly IProductReviewRepository _reviewRepository;
            private readonly ReviewManager _reviewManager;
            private readonly ILogger<ReviewController> _logger;

            public ReviewController(IProductReviewRepository reviewRepository, ReviewManager reviewManager, ILogger<ReviewController> logger)
            {
                _reviewRepository = reviewRepository;
                _reviewManager = reviewManager;
                _logger = logger;
            }

            // Add Review
            [HttpPost]
            public async Task<IActionResult> AddReview([FromBody] ProductReview review)
            {
                if (review == null)
                {
                    return BadRequest("Review object is required.");
                }

                try
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);

                    if (!_reviewManager.ValidateReview(review))
                        return BadRequest(new { message = "Invalid review details" });

                    var reviewEntity = new ProductReviewEntity
                    {
                        ProductId = review.ProductId,
                        UserId = review.UserId,
                        Rating = review.Rating,
                        Title = review.Title,
                        Comment = review.Comment,
                        IsVerifiedPurchase = review.IsVerifiedPurchase,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    var result = await _reviewRepository.AddAsync(reviewEntity);
                    return result != null
                        ? Ok(new { message = "Review added successfully" })
                        : BadRequest(new { message = "Failed to add review" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding review");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Get Reviews by Product ID
            [HttpGet("product/{productId}")]
            public async Task<IActionResult> GetReviewsByProductId(int productId)
            {
                try
                {
                    var reviews = await _reviewRepository.GetByProductIdAsync(productId);
                    if (!reviews.Any()) return NotFound(new { message = "No reviews found for this product." });

                    var reviewDtos = reviews.Adapt<List<ProductReview>>();
                    return Ok(reviewDtos);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting reviews by product ID: {ProductId}", productId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Get Reviews by User ID
            [HttpGet("user/{userId}")]
            public async Task<IActionResult> GetReviewsByUserId(string userId)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is required.");
                }

                try
                {
                    var reviews = await _reviewRepository.GetByUserIdAsync(userId);
                    if (!reviews.Any()) return NotFound(new { message = "No reviews found for this user." });

                    var reviewDtos = reviews.Adapt<List<ProductReview>>();
                    return Ok(reviewDtos);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting reviews by user ID: {UserId}", userId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Delete Review
            [HttpDelete("{reviewId}")]
            public async Task<IActionResult> DeleteReview(int reviewId)
            {
                try
                {
                    var result = await _reviewRepository.DeleteAsync(reviewId);
                    return result
                        ? Ok(new { message = "Review deleted successfully" })
                        : BadRequest(new { message = "Failed to delete review" });
                }
                catch (KeyNotFoundException)
                {
                    return NotFound(new { message = "Review not found." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting review with ID: {ReviewId}", reviewId);
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
