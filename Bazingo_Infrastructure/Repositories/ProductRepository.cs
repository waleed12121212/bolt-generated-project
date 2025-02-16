    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Product;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly DbSet<ProductEntity> _products;
            private readonly ILogger<ProductRepository> _logger;

            public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger) : base(context)
            {
                _context = context;
                _products = context.Set<ProductEntity>();
                _logger = logger;
            }

            public async Task<ProductEntity> GetByIdAsync(int id)
            {
                try
                {
                    return await _products
                        .Include(p => p.Category)
                        .Include(p => p.Images)
                        .Include(p => p.Attributes)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting product by ID {ProductId}", id);
                    return null;
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetAllAsync()
            {
                try
                {
                    return await _products
                        .Include(p => p.Category)
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all products");
                    return new List<ProductEntity>();
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetByCategoryAsync(int categoryId)
            {
                try
                {
                    return await _products
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting products by category {CategoryId}", categoryId);
                    return new List<ProductEntity>();
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetBySellerAsync(string sellerId)
            {
                try
                {
                    return await _products
                        .Include(p => p.Category)
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => p.SellerId == sellerId && !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting products for seller {SellerId}", sellerId);
                    return new List<ProductEntity>();
                }
            }

            public async Task<ProductEntity> AddAsync(ProductEntity product)
            {
                try
                {
                    product.CreatedAt = DateTime.UtcNow;
                    product.LastUpdated = DateTime.UtcNow;
                    await _products.AddAsync(product);
                    await _context.SaveChangesAsync();
                    return product;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding product {ProductName}", product.Name);
                    return null;
                }
            }

            public async Task<bool> UpdateAsync(ProductEntity product)
            {
                try
                {
                    product.LastUpdated = DateTime.UtcNow;
                    _context.Entry(product).State = EntityState.Modified;
                    return await _context.SaveChangesAsync() > 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product {ProductId}", product.Id);
                    return false;
                }
            }

            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    var product = await GetByIdAsync(id);
                    if (product != null)
                    {
                        product.IsDeleted = true;
                        product.LastUpdated = DateTime.UtcNow;
                        return await UpdateAsync(product);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting product {ProductId}", id);
                    return false;
                }
            }

            public async Task<IEnumerable<ProductEntity>> SearchAsync(string keyword, int? categoryId = null)
            {
                try
                {
                    var query = _products.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query = query.Where(p =>
                            p.Name.Contains(keyword) ||
                            p.Description.Contains(keyword) ||
                            p.Brand.Contains(keyword));
                    }

                    if (categoryId.HasValue)
                    {
                        query = query.Where(p => p.CategoryId == categoryId.Value);
                    }

                    return await query
                        .Include(p => p.Category)
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error searching products with term {SearchTerm}", keyword);
                    return new List<ProductEntity>();
                }
            }

            public async Task<bool> UpdateStockAsync(int productId, int quantity)
            {
                try
                {
                    var product = await _products.FindAsync(productId);
                    if (product != null)
                    {
                        product.StockQuantity = quantity;
                        product.LastUpdated = DateTime.UtcNow;
                        return await _context.SaveChangesAsync() > 0;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating stock for product {ProductId}", productId);
                    return false;
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetFeaturedProductsAsync()
            {
                try
                {
                    return await _products
                        .Include(p => p.Category)
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => p.IsFeatured && !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(10)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting featured products", ex);
                    return new List<ProductEntity>();
                }
            }

            public async Task<IEnumerable<ProductEntity>> GetNewArrivalsAsync()
            {
                try
                {
                    return await _products
                        .Include(p => p.Category)
                        .Include(p => p.Images.Where(i => i.IsPrimary))
                        .Where(p => !p.IsDeleted)
                        .OrderByDescending(p => p.CreatedAt)
                        .Take(10)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting new arrivals", ex);
                    return new List<ProductEntity>();
                }
            }

            public async Task<decimal> GetLowestPriceAsync(int productId)
            {
                try
                {
                    var product = await _products.FindAsync(productId);
                    return product?.Price ?? 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting lowest price for product {ProductId}", productId);
                    return 0;
                }
            }

            public async Task<decimal> GetHighestPriceAsync(int productId)
            {
                try
                {
                    var product = await _products.FindAsync(productId);
                    return product?.Price ?? 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting highest price for product {ProductId}", productId);
                    return 0;
                }
            }
        }
    }
