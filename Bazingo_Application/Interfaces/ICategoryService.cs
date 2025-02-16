using Bazingo_Application.DTOs.Category;
using Bazingo_Core.Models.Common;

namespace Bazingo_Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(int id);
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetMainCategoriesAsync();
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetSubCategoriesAsync(int parentId);
        Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto);
        Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        Task<ApiResponse<bool>> DeleteCategoryAsync(int id);
    }
}
