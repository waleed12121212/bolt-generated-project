    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Product;

    namespace Bazingo_Core.Interfaces
    {
        public interface IProductRepository
        {
            Task<ProductEntity> GetByIdAsync(int id);
            Task<IEnumerable<ProductEntity>> GetAllAsync();
            Task<IEnumerable<ProductEntity>> GetByCategoryAsync(int categoryId);
            Task<IEnumerable<ProductEntity>> GetBySellerAsync(string sellerId);
            Task<IEnumerable<ProductEntity>> SearchAsync(string keyword, int? categoryId = null);
            Task<ProductEntity> AddAsync(ProductEntity product);
            Task<bool> UpdateAsync(ProductEntity product);
            Task<bool> DeleteAsync(int id);
            Task<bool> UpdateStockAsync(int productId, int quantity);
            Task<IEnumerable<ProductEntity>> GetFeaturedProductsAsync();
            Task<IEnumerable<ProductEntity>> GetNewArrivalsAsync();
            Task<decimal> GetLowestPriceAsync(int productId);
            Task<decimal> GetHighestPriceAsync(int productId);
        }
    }
