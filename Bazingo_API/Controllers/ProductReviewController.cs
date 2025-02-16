using Bazingo_Application.DTOs.ProductReview;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_API.Controllers
{
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
            return await _reviewService.GetReviewByIdAsync(id);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<ApiResponse<List<ProductReviewDto>>>> GetProductReviews(int productId)
        {
            return await _reviewService.GetProductReviewsAsync(productId);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<ProductReviewDto>>>> GetUserReviews()
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _reviewService.GetUserReviewsAsync(userId);
        }

        [HttpGet("product/{productId}/summary")]
        public async Task<ActionResult<ApiResponse<ProductReviewSummaryDto>>> GetProductReviewSummary(int productId)
        {
            return await _reviewService.GetProductReviewSummaryAsync(productId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ProductReviewDto>>> CreateReview([FromBody] CreateProductReviewDto dto)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _reviewService.CreateReviewAsync(dto, userId);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ProductReviewDto>>> UpdateReview(int id, [FromBody] UpdateProductReviewDto dto)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _reviewService.UpdateReviewAsync(id, dto, userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteReview(int id)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _reviewService.DeleteReviewAsync(id, userId);
        }
    }
}
