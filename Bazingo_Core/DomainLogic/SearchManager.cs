using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Core.DomainLogic
{
    public class SearchManager
    {
        public IQueryable<ProductEntity> SearchProducts(IQueryable<ProductEntity> products, string searchTerm,
            decimal? minPrice = null, decimal? maxPrice = null,
            string category = null, string brand = null,
            bool? inStock = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;

            var query = products;

            // Apply search term
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Description.Contains(searchTerm) ||
                p.Brand.Contains(searchTerm));

            // Apply filters
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category.Name == category);

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(p => p.Brand == brand);

            if (inStock.HasValue)
                query = query.Where(p => p.StockQuantity > 0 == inStock.Value);

            return query;
        }

        public IQueryable<ProductEntity> SortProducts(IQueryable<ProductEntity> products, string sortBy, bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return products;

            switch (sortBy.ToLower())
            {
                case "price":
                    return ascending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price);
                case "name":
                    return ascending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name);
                case "date":
                    return ascending ? products.OrderBy(p => p.CreatedAt) : products.OrderByDescending(p => p.CreatedAt);
                case "popularity":
                    return ascending ? products.OrderBy(p => p.ViewCount) : products.OrderByDescending(p => p.ViewCount);
                default:
                    return products;
            }
        }

        public IQueryable<ProductEntity> FilterProductsByCategory(IQueryable<ProductEntity> products, int categoryId)
        {
            return products.Where(p => p.CategoryId == categoryId);
        }

        public IQueryable<ProductEntity> FilterProductsByPriceRange(IQueryable<ProductEntity> products, decimal minPrice, decimal maxPrice)
        {
            return products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        public IQueryable<ProductEntity> FilterProductsByBrand(IQueryable<ProductEntity> products, string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
                return products;

            return products.Where(p => p.Brand == brand);
        }

        public IQueryable<ProductEntity> FilterProductsByAvailability(IQueryable<ProductEntity> products, bool inStockOnly)
        {
            return inStockOnly ? products.Where(p => p.StockQuantity > 0) : products;
        }
    }
}
