using Bazingo_Application.DTOs.Product;
using Bazingo_Application.DTOs.Search;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService , ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        /// <summary>
        /// Search products with filtering, sorting, and pagination
        /// </summary>
        [HttpGet("products")]
        [ProducesResponseType(typeof(ApiResponse<SearchResultDto<ProductDto>>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SearchResultDto<ProductDto>>) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<SearchResultDto<ProductDto>>>> SearchProducts([FromQuery] SearchDto searchDto)
        {
            var result = await _searchService.SearchProductsAsync(searchDto);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get search suggestions based on a query
        /// </summary>
        [HttpGet("suggestions")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<List<string>>>> GetSearchSuggestions([FromQuery] string query)
        {
            var result = await _searchService.GetSearchSuggestionsAsync(query);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get available search filters
        /// </summary>
        [HttpGet("filters")]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string , List<string>>>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Dictionary<string , List<string>>>) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<Dictionary<string , List<string>>>>> GetSearchFilters( )
        {
            var result = await _searchService.GetSearchFiltersAsync();
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Get search metadata
        /// </summary>
        [HttpGet("metadata")]
        [ProducesResponseType(typeof(ApiResponse<SearchMetadataDto>) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<SearchMetadataDto>) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<SearchMetadataDto>>> GetSearchMetadata( )
        {
            var result = await _searchService.GetSearchMetadataAsync();
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
