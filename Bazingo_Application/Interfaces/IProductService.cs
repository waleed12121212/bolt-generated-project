using Bazingo_Application.DTOs.Product;
using Bazingo_Core.Models.Common;

namespace Bazingo_Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
        Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(int categoryId);
        Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsBySellerAsync(string sellerId);
        Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm);
        Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto dto, string sellerId);
        Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<bool>> UpdateStockAsync(int id, int quantity);
    }
}
