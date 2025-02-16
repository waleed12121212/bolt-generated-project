using Bazingo_Application.DTOs.Category;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Entities;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Bazingo_Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return ApiResponse<CategoryDto>.CreateError("Category not found");

                var categoryDto = category.Adapt<CategoryDto>();
                return ApiResponse<CategoryDto>.CreateSuccess(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category with ID: {Id}", id);
                return ApiResponse<CategoryDto>.CreateError("Error retrieving category");
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var categoryDtos = categories.Adapt<IEnumerable<CategoryDto>>();
                return ApiResponse<IEnumerable<CategoryDto>>.CreateSuccess(categoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                return ApiResponse<IEnumerable<CategoryDto>>.CreateError("Error retrieving categories");
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetMainCategoriesAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var mainCategories = categories.Where(c => c.ParentCategoryId == null);
                var categoryDtos = mainCategories.Adapt<IEnumerable<CategoryDto>>();
                return ApiResponse<IEnumerable<CategoryDto>>.CreateSuccess(categoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting main categories");
                return ApiResponse<IEnumerable<CategoryDto>>.CreateError("Error retrieving main categories");
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetSubCategoriesAsync(int parentId)
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var subCategories = categories.Where(c => c.ParentCategoryId == parentId);
                var categoryDtos = subCategories.Adapt<IEnumerable<CategoryDto>>();
                return ApiResponse<IEnumerable<CategoryDto>>.CreateSuccess(categoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting subcategories for parent ID: {ParentId}", parentId);
                return ApiResponse<IEnumerable<CategoryDto>>.CreateError("Error retrieving subcategories");
            }
        }

        public async Task<ApiResponse<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                if (dto.ParentCategoryId.HasValue)
                {
                    var parentExists = await _unitOfWork.Categories.GetByIdAsync(dto.ParentCategoryId.Value) != null;
                    if (!parentExists)
                        return ApiResponse<CategoryDto>.CreateError("Parent category not found");
                }

                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl,
                    ParentCategoryId = dto.ParentCategoryId
                    // IsActive is set to true by default in the constructor
                };

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CompleteAsync();

                var categoryDto = category.Adapt<CategoryDto>();
                return ApiResponse<CategoryDto>.CreateSuccess(categoryDto, "Category created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return ApiResponse<CategoryDto>.CreateError("Error creating category");
            }
        }

        public async Task<ApiResponse<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return ApiResponse<CategoryDto>.CreateError("Category not found");

                if (dto.ParentCategoryId.HasValue && dto.ParentCategoryId.Value != category.ParentCategoryId)
                {
                    var parentExists = await _unitOfWork.Categories.GetByIdAsync(dto.ParentCategoryId.Value) != null;
                    if (!parentExists)
                        return ApiResponse<CategoryDto>.CreateError("Parent category not found");
                }

                category.Name = dto.Name;
                category.Description = dto.Description;
                category.ImageUrl = dto.ImageUrl;
                category.ParentCategoryId = dto.ParentCategoryId;
                category.IsActive = dto.IsActive;

                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.CompleteAsync();

                var categoryDto = category.Adapt<CategoryDto>();
                return ApiResponse<CategoryDto>.CreateSuccess(categoryDto, "Category updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID: {Id}", id);
                return ApiResponse<CategoryDto>.CreateError("Error updating category");
            }
        }

        public async Task<ApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                    return ApiResponse<bool>.CreateError("Category not found");

                var hasProducts = await _unitOfWork.Products.GetByCategoryAsync(id) != null;
                if (hasProducts)
                    return ApiResponse<bool>.CreateError("Cannot delete category with associated products");

                var hasSubCategories = await _unitOfWork.Categories.GetAllAsync();
                if (hasSubCategories.Any(c => c.ParentCategoryId == id))
                    return ApiResponse<bool>.CreateError("Cannot delete category with subcategories");

                await _unitOfWork.Categories.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Category deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
                return ApiResponse<bool>.CreateError("Error deleting category");
            }
        }
    }
}
