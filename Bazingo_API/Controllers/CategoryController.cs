using Bazingo_Application.DTOs.Category;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazingo_API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllCategories()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }

        [HttpGet("main")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetMainCategories()
        {
            return await _categoryService.GetMainCategoriesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(int id)
        {
            return await _categoryService.GetCategoryByIdAsync(id);
        }

        [HttpGet("{parentId}/subcategories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetSubCategories(int parentId)
        {
            return await _categoryService.GetSubCategoriesAsync(parentId);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            return await _categoryService.CreateCategoryAsync(dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            return await _categoryService.UpdateCategoryAsync(id, dto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(int id)
        {
            return await _categoryService.DeleteCategoryAsync(id);
        }
    }
}
