    using Bazingo_Core.Interfaces;
    using Bazingo_Core.Entities.Product;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace Bazingo_Application.Services.Core
    {
        public class ProductCoreService : IProductService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<ProductCoreService> _logger;

            public ProductCoreService(IUnitOfWork unitOfWork, ILogger<ProductCoreService> logger)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<ProductEntity> GetProductByIdAsync(int productId)
            {
                try
                {
                    return await _unitOfWork.Products.GetByIdAsync(productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting product by ID {ProductId}", productId);
                    throw;
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
            {
                try
                {
                    return await _unitOfWork.Products.GetAllAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all products");
                    throw;
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetProductsByCategoryAsync(int categoryId)
            {
                try
                {
                    return await _unitOfWork.Products.GetByCategoryAsync(categoryId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting products by category {CategoryId}", categoryId);
                    throw;
                }
            }

            public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
            {
                try
                {
                    await _unitOfWork.Products.AddAsync(product);
                    await _unitOfWork.CompleteAsync();
                    return product;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product {ProductName}", product.Name);
                    throw;
                }
            }

            public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
            {
                try
                {
                    var success = await _unitOfWork.Products.UpdateAsync(product);
                    if (!success)
                    {
                        throw new InvalidOperationException($"Failed to update product with ID {product.Id}");
                    }
                    await _unitOfWork.CompleteAsync();
                    return product;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product {ProductId}", product.Id);
                    throw;
                }
            }

            public async Task<bool> DeleteProductAsync(int productId)
            {
                try
                {
                    var success = await _unitOfWork.Products.DeleteAsync(productId);
                    if (success)
                    {
                        await _unitOfWork.CompleteAsync();
                    }
                    return success;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting product {ProductId}", productId);
                    throw;
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetProductsBySellerAsync(string sellerId)
            {
                try
                {
                    return await _unitOfWork.Products.GetBySellerAsync(sellerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting products by seller {SellerId}", sellerId);
                    throw;
                }
            }

            public async Task<IEnumerable<ProductEntity>> SearchProductsAsync(string searchTerm)
            {
                try
                {
                    return await _unitOfWork.Products.SearchAsync(searchTerm);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error searching products with term {SearchTerm}", searchTerm);
                    throw;
                }
            }
        }
    }
