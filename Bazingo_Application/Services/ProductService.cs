using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bazingo_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductEntity> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.Products.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Products.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsBySellerAsync(string sellerId)
        {
            try
            {
                return await _unitOfWork.Products.GetBySellerAsync(sellerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products for seller {SellerId}", sellerId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductEntity>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                return await _unitOfWork.Products.SearchAsync(searchTerm, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
        {
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var success = await _unitOfWork.Products.DeleteAsync(productId);
            if (success)
            {
                await _unitOfWork.CompleteAsync();
            }
            return success;
        }
    }
}
