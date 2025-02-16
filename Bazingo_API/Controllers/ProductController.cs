    using Bazingo_Application.DTOs.Product;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        public class ProductController : BaseController
        {
            private readonly IProductService _productService;
            private readonly ILogger<ProductController> _logger;

            public ProductController(IProductService productService, ILogger<ProductController> logger)
            {
                _productService = productService;
                _logger = logger;
            }

            [HttpGet]
            public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAllProducts()
            {
                return await _productService.GetAllProductsAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(int id)
            {
                return await _productService.GetProductByIdAsync(id);
            }

            [HttpGet("category/{categoryId}")]
            public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsByCategory(int categoryId)
            {
                return await _productService.GetProductsByCategoryAsync(categoryId);
            }

            [HttpGet("seller/{sellerId}")]
            public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsBySeller(string sellerId)
            {
                return await _productService.GetProductsBySellerAsync(sellerId);
            }

            [HttpGet("search")]
            public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> SearchProducts([FromQuery] string searchTerm)
            {
                return await _productService.SearchProductsAsync(searchTerm);
            }

            [HttpPost]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto dto)
            {
                var sellerId = User.FindFirst("sub")?.Value;
                return await _productService.CreateProductAsync(dto, sellerId);
            }

            [HttpPut("{id}")]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
            {
                return await _productService.UpdateProductAsync(id, dto);
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
            {
                return await _productService.DeleteProductAsync(id);
            }

            [HttpPatch("{id}/stock")]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(int id, [FromBody] int quantity)
            {
                return await _productService.UpdateStockAsync(id, quantity);
            }
        }
    }
