    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bazingo_Application.DTOs.ProductReview;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [Authorize]
        public class ProductReviewController : BaseController
        {
            private readonly IProductReviewService _reviewService;
            private readonly ILogger<ProductReviewController> _logger;

            public ProductReviewController(IProductReviewService reviewService, ILogger<ProductReviewController> logger)
            {
                _reviewService = reviewService;
                _logger = logger;
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ApiResponse<ProductReviewDto>>> GetReview(int id)
            {
                try
                {
                    return await _reviewService.GetReviewByIdAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting review by ID: {ReviewId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("product/{productId}")]
            public async Task<ActionResult<ApiResponse<List<ProductReviewDto>>>> GetProductReviews(int productId)
            {
                try
                {
                    return await _reviewService.GetProductReviewsAsync(productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting product reviews for product ID: {ProductId}", productId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("user")]
            public async Task<ActionResult<ApiResponse<List<ProductReviewDto>>>> GetUserReviews()
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _reviewService.GetUserReviewsAsync(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user reviews");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("product/{productId}/summary")]
            public async Task<ActionResult<ApiResponse<ProductReviewSummaryDto>>> GetProductReviewSummary(int productId)
            {
                try
                {
                    return await _reviewService.GetProductReviewSummaryAsync(productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting product review summary for product ID: {ProductId}", productId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost]
            public async Task<ActionResult<ApiResponse<ProductReviewDto>>> CreateReview([FromBody] CreateProductReviewDto dto)
            {
                if (dto == null)
                {
                    return BadRequest("CreateProductReviewDto object is required.");
                }

                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _reviewService.CreateReviewAsync(dto, userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating review");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("{id}")]
            public async Task<ActionResult<ApiResponse<ProductReviewDto>>> UpdateReview(int id, [FromBody] UpdateProductReviewDto dto)
            {
                if (dto == null)
                {
                    return BadRequest("UpdateProductReviewDto object is required.");
                }

                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _reviewService.UpdateReviewAsync(id, dto, userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating review with ID: {ReviewId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult<ApiResponse<bool>>> DeleteReview(int id)
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _reviewService.DeleteReviewAsync(id, userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting review with ID: {ReviewId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
