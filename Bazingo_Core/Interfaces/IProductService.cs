    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Bazingo_Core.Entities.Product;

    namespace Bazingo_Core.Interfaces
    {
        public interface IProductService
        {
            Task<ProductEntity> GetProductByIdAsync(int productId);
            Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
            Task<IEnumerable<ProductEntity>> GetProductsByCategoryAsync(int categoryId);
            Task<IEnumerable<ProductEntity>> GetProductsBySellerAsync(string sellerId);
            Task<IEnumerable<ProductEntity>> SearchProductsAsync(string searchTerm);
            Task<ProductEntity> CreateProductAsync(ProductEntity product);
            Task<ProductEntity> UpdateProductAsync(ProductEntity product);
            Task<bool> DeleteProductAsync(int productId);
        }
    }
