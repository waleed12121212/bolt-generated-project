using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Application.DTOs.ProductReview;
using Bazingo_Core.Models.Common;

namespace Bazingo_Application.Interfaces
{
    public interface IProductReviewService
    {
        Task<ApiResponse<ProductReviewDto>> GetReviewByIdAsync(int id);
        Task<ApiResponse<List<ProductReviewDto>>> GetProductReviewsAsync(int productId);
        Task<ApiResponse<List<ProductReviewDto>>> GetUserReviewsAsync(string userId);
        Task<ApiResponse<ProductReviewSummaryDto>> GetProductReviewSummaryAsync(int productId);
        Task<ApiResponse<ProductReviewDto>> CreateReviewAsync(CreateProductReviewDto dto, string userId);
        Task<ApiResponse<ProductReviewDto>> UpdateReviewAsync(int id, UpdateProductReviewDto dto, string userId);
        Task<ApiResponse<bool>> DeleteReviewAsync(int id, string userId);
        
        // Additional helper methods
        Task<ApiResponse<double>> GetAverageRatingAsync(int productId);
        Task<ApiResponse<int>> GetReviewCountAsync(int productId);
        Task<ApiResponse<bool>> HasUserReviewedAsync(int productId, string userId);
        Task<ApiResponse<List<ProductReviewDto>>> GetRecentReviewsAsync(int count);
        Task<ApiResponse<List<ProductReviewDto>>> GetTopRatedReviewsAsync(int count);
        Task<ApiResponse<List<ProductReviewDto>>> GetVerifiedReviewsAsync(int productId);
        Task<ApiResponse<bool>> MarkAsVerifiedPurchaseAsync(int reviewId);
    }
}
