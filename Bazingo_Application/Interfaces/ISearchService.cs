using Bazingo_Application.DTOs.Product;
using Bazingo_Application.DTOs.Search;
using Bazingo_Core.Models.Common;

namespace Bazingo_Application.Interfaces
{
    public interface ISearchService
    {
        Task<ApiResponse<SearchResultDto<ProductDto>>> SearchProductsAsync(SearchDto searchDto);
        Task<ApiResponse<List<string>>> GetSearchSuggestionsAsync(string query);
        Task<ApiResponse<SearchFiltersDto>> GetSearchFiltersAsync();
        Task<ApiResponse<SearchMetadataDto>> GetSearchMetadataAsync();
    }
}
