using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bazingo_Application.DTOs.Product;
using Bazingo_Application.DTOs.Search;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bazingo_Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<SearchService> _logger;
        private const int DefaultPageSize = 20;

        public SearchService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ILogger<SearchService> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<SearchResultDto<ProductDto>>> SearchProductsAsync(SearchDto searchDto)
        {
            try
            {
                var products = await _productRepository.SearchAsync(
                    searchDto.Query,
                    searchDto.CategoryId);

                // Convert to IQueryable for efficient filtering and sorting
                var queryableProducts = products.AsQueryable();

                // Apply filters
                if (searchDto.MinPrice.HasValue || searchDto.MaxPrice.HasValue || searchDto.MinRating.HasValue)
                {
                    queryableProducts = queryableProducts.Where(p =>
                        (!searchDto.MinPrice.HasValue || p.Price >= searchDto.MinPrice.Value) &&
                        (!searchDto.MaxPrice.HasValue || p.Price <= searchDto.MaxPrice.Value) &&
                        (!searchDto.MinRating.HasValue || p.AverageRating >= searchDto.MinRating.Value));
                }

                // Apply sorting
                queryableProducts = ApplySorting(queryableProducts, searchDto.SortBy, searchDto.SortDirection);

                // Apply pagination
                var totalItems = queryableProducts.Count();
                var pageSize = searchDto.PageSize > 0 ? searchDto.PageSize : DefaultPageSize;
                var currentPage = searchDto.Page > 0 ? searchDto.Page : 1;
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var pagedProducts = queryableProducts
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var metadata = new SearchMetadataDto
                {
                    TotalProducts = totalItems,
                    TotalCategories = (await _categoryRepository.GetAllAsync()).Count(),
                    TotalBrands = queryableProducts.Select(p => p.Brand).Distinct().Count(),
                    MinPrice = queryableProducts.Any() ? queryableProducts.Min(p => p.Price) : 0,
                    MaxPrice = queryableProducts.Any() ? queryableProducts.Max(p => p.Price) : 0,
                    CategoryDistribution = queryableProducts
                        .GroupBy(p => p.Category.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BrandDistribution = queryableProducts
                        .GroupBy(p => p.Brand)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    PopularSearchTerms = await GetPopularSearchTermsAsync()
                };

                var result = new SearchResultDto<ProductDto>
                {
                    Items = pagedProducts.Adapt<List<ProductDto>>(),
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    CurrentPage = currentPage,
                    HasNextPage = currentPage < totalPages,
                    HasPreviousPage = currentPage > 1,
                    Facets = await GetFacetsAsync(queryableProducts),
                    Metadata = metadata
                };

                return ApiResponse<SearchResultDto<ProductDto>>.CreateSuccess(result, "Search completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching products");
                return ApiResponse<SearchResultDto<ProductDto>>.CreateError("Error occurred while searching products");
            }
        }

        private IQueryable<ProductEntity> ApplySorting(IQueryable<ProductEntity> products, string sortBy, SortDirection sortDirection)
        {
            var isAscending = sortDirection == SortDirection.Ascending;

            return sortBy?.ToLower() switch
            {
                "price" => isAscending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price),
                "name" => isAscending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name),
                "rating" => isAscending ? products.OrderBy(p => p.AverageRating) : products.OrderByDescending(p => p.AverageRating),
                "date" => isAscending ? products.OrderBy(p => p.CreatedAt) : products.OrderByDescending(p => p.CreatedAt),
                _ => products.OrderByDescending(p => p.CreatedAt) // Default sorting
            };
        }

        private async Task<Dictionary<string, object>> GetFacetsAsync(IQueryable<ProductEntity> products)
        {
            var facets = new Dictionary<string, object>();

            // Get price ranges
            var priceStats = await products.GroupBy(x => 1).Select(g => new
            {
                MinPrice = g.Min(p => p.Price),
                MaxPrice = g.Max(p => p.Price),
                AvgPrice = g.Average(p => p.Price)
            }).FirstOrDefaultAsync();

            if (priceStats != null)
            {
                facets.Add("priceRanges", new
                {
                    min = priceStats.MinPrice,
                    max = priceStats.MaxPrice,
                    avg = priceStats.AvgPrice
                });
            }

            // Get brand distribution
            var brands = await products
                .GroupBy(p => p.Brand)
                .Select(g => new { Brand = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            facets.Add("brands", brands);

            // Get category distribution
            var categories = await products
                .GroupBy(p => p.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            facets.Add("categories", categories);

            // Get rating distribution
            var ratings = await products
                .GroupBy(p => Math.Floor(p.AverageRating))
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Rating)
                .ToListAsync();

            facets.Add("ratings", ratings);

            return facets;
        }

        public async Task<ApiResponse<List<string>>> GetSearchSuggestionsAsync(string query)
        {
            try
            {
                var products = await _productRepository.SearchAsync(query, null);
                var suggestions = products
                    .Select(p => p.Name)
                    .Take(10)
                    .ToList();

                return ApiResponse<List<string>>.CreateSuccess(suggestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting search suggestions");
                return ApiResponse<List<string>>.CreateError("Error occurred while getting search suggestions");
            }
        }

        public async Task<ApiResponse<SearchFiltersDto>> GetSearchFiltersAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var products = await _productRepository.GetAllAsync();

                var filters = new SearchFiltersDto
                {
                    Categories = categories.Select(c => c.Name).ToList(),
                    Brands = products.Select(p => p.Brand).Distinct().ToList(),
                    MinPrice = products.Any() ? products.Min(p => p.Price) : 0,
                    MaxPrice = products.Any() ? products.Max(p => p.Price) : 0
                };

                return ApiResponse<SearchFiltersDto>.CreateSuccess(filters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting search filters");
                return ApiResponse<SearchFiltersDto>.CreateError("Error occurred while getting search filters");
            }
        }

        public async Task<ApiResponse<SearchMetadataDto>> GetSearchMetadataAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var categories = await _categoryRepository.GetAllAsync();

                var metadata = new SearchMetadataDto
                {
                    TotalProducts = products.Count(),
                    TotalCategories = categories.Count(),
                    TotalBrands = products.Select(p => p.Brand).Distinct().Count(),
                    MinPrice = products.Any() ? products.Min(p => p.Price) : 0,
                    MaxPrice = products.Any() ? products.Max(p => p.Price) : 0,
                    CategoryDistribution = products
                        .GroupBy(p => p.Category.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BrandDistribution = products
                        .GroupBy(p => p.Brand)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    PopularSearchTerms = await GetPopularSearchTermsAsync()
                };

                return ApiResponse<SearchMetadataDto>.CreateSuccess(metadata, "Search metadata retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting search metadata");
                return ApiResponse<SearchMetadataDto>.CreateError("Error occurred while getting search metadata");
            }
        }

        private async Task<List<string>> GetPopularSearchTermsAsync()
        {
            // TODO: Implement actual popular search terms tracking
            // For now, return empty list
            return new List<string>();
        }
    }
}
