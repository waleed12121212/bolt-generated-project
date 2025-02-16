using Bazingo_Application.DTOs.Product;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Bazingo_Application.Services
{
    public class ProductApplicationService : Bazingo_Application.Interfaces.IProductService
    {
        private readonly Bazingo_Core.Interfaces.IProductService _productService;
        private readonly ILogger<ProductApplicationService> _logger;

        public ProductApplicationService(Bazingo_Core.Interfaces.IProductService productService, ILogger<ProductApplicationService> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        private ProductDto MapToDto(ProductEntity product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl ?? string.Empty,
                SKU = product.SKU,
                Brand = product.Brand,
                IsAvailable = product.IsAvailable,
                IsFeatured = product.IsFeatured,
                ViewCount = product.ViewCount,
                AverageRating = product.AverageRating,
                Status = product.Status,
                SellerId = product.SellerId,
                SellerName = product.Seller?.UserName,
                CategoryName = product.Category?.Name,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.LastUpdated,
                ImageUrls = product.Images?.Select(i => i.ImageUrl).ToList() ?? new List<string>(),
                Attributes = product.Attributes?.ToDictionary(a => a.Name, a => a.Value) ?? new Dictionary<string, string>(),
                ReviewsCount = product.Reviews?.Count ?? 0
            };
        }

        public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return ApiResponse<ProductDto>.CreateError("Product not found");
                }

                return ApiResponse<ProductDto>.CreateSuccess(MapToDto(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by ID {ProductId}", id);
                return ApiResponse<ProductDto>.CreateError("Error retrieving product");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                var productDtos = products.Select(MapToDto);
                return ApiResponse<IEnumerable<ProductDto>>.CreateSuccess(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all products");
                return ApiResponse<IEnumerable<ProductDto>>.CreateError("Error retrieving products");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryAsync(categoryId);
                var productDtos = products.Select(MapToDto);
                return ApiResponse<IEnumerable<ProductDto>>.CreateSuccess(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by category {CategoryId}", categoryId);
                return ApiResponse<IEnumerable<ProductDto>>.CreateError("Error retrieving products");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsBySellerAsync(string sellerId)
        {
            try
            {
                var products = await _productService.GetProductsBySellerAsync(sellerId);
                var productDtos = products.Select(MapToDto);
                return ApiResponse<IEnumerable<ProductDto>>.CreateSuccess(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by seller {SellerId}", sellerId);
                return ApiResponse<IEnumerable<ProductDto>>.CreateError("Error retrieving products");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                var products = await _productService.SearchProductsAsync(searchTerm);
                var productDtos = products.Select(MapToDto);
                return ApiResponse<IEnumerable<ProductDto>>.CreateSuccess(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term {SearchTerm}", searchTerm);
                return ApiResponse<IEnumerable<ProductDto>>.CreateError("Error searching products");
            }
        }

        public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto dto, string sellerId)
        {
            try
            {
                var product = new ProductEntity
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price ?? 0m,
                    CategoryId = dto.CategoryId ?? 0,
                    StockQuantity = dto.StockQuantity ?? 0,
                    ImageUrl = dto.ImageUrl,
                    SellerId = sellerId,
                    SKU = dto.SKU,
                    Brand = dto.Brand,
                    Status = dto.Status ?? ProductStatus.Draft,
                    IsAvailable = true
                };

                var createdProduct = await _productService.CreateProductAsync(product);
                return await GetProductByIdAsync(createdProduct.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product for seller {SellerId}", sellerId);
                return ApiResponse<ProductDto>.CreateError("Error creating product");
            }
        }

        public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return ApiResponse<ProductDto>.CreateError("Product not found");
                }

                if (!string.IsNullOrEmpty(dto.Name))
                    existingProduct.Name = dto.Name;
                if (!string.IsNullOrEmpty(dto.Description))
                    existingProduct.Description = dto.Description;
                if (dto.Price.HasValue)
                    existingProduct.Price = dto.Price.Value;
                if (dto.CategoryId.HasValue)
                    existingProduct.CategoryId = dto.CategoryId.Value;
                if (dto.StockQuantity.HasValue)
                    existingProduct.StockQuantity = dto.StockQuantity.Value;
                if (!string.IsNullOrEmpty(dto.ImageUrl))
                    existingProduct.ImageUrl = dto.ImageUrl;
                if (!string.IsNullOrEmpty(dto.SKU))
                    existingProduct.SKU = dto.SKU;
                if (!string.IsNullOrEmpty(dto.Brand))
                    existingProduct.Brand = dto.Brand;
                if (dto.Status.HasValue)
                    existingProduct.Status = dto.Status.Value;

                existingProduct.LastUpdated = DateTime.UtcNow;

                var updatedProduct = await _productService.UpdateProductAsync(existingProduct);
                return await GetProductByIdAsync(updatedProduct.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return ApiResponse<ProductDto>.CreateError("Error updating product");
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return ApiResponse<bool>.CreateError("Product not found");
                }

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return ApiResponse<bool>.CreateError("Error deleting product");
            }
        }

        public async Task<ApiResponse<bool>> UpdateStockAsync(int id, int quantity)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return ApiResponse<bool>.CreateError("Product not found");
                }

                product.StockQuantity = quantity;
                product.LastUpdated = DateTime.UtcNow;
                await _productService.UpdateProductAsync(product);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock for product {ProductId}", id);
                return ApiResponse<bool>.CreateError("Error updating stock");
            }
        }
    }
}
