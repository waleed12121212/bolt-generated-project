    using Bazingo_Application.DTOs.Product;
    using Bazingo_Application.DTOs.Search;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class SearchController : ControllerBase
        {
            private readonly ISearchService _searchService;
            private readonly ILogger<SearchController> _logger;

            public SearchController(ISearchService searchService, ILogger<SearchController> logger)
            {
                _searchService = searchService;
                _logger = logger;
            }

            /// <summary>
            /// Search products with filtering, sorting, and pagination
            /// </summary>
            [HttpGet("products")]
            [ProducesResponseType(typeof(ApiResponse<SearchResultDto<ProductDto>>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<SearchResultDto<ProductDto>>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<SearchResultDto<ProductDto>>>> SearchProducts([FromQuery] SearchDto searchDto)
            {
                try
                {
                    var result = await _searchService.SearchProductsAsync(searchDto);
                    if (!result.Succeeded)
                        return BadRequest(result);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while searching products");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            /// <summary>
            /// Get search suggestions based on a query
            /// </summary>
            [HttpGet("suggestions")]
            [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<List<string>>>> GetSearchSuggestions([FromQuery] string query)
            {
                try
                {
                    if (string.IsNullOrEmpty(query))
                    {
                        return BadRequest("Search query is required.");
                    }

                    var result = await _searchService.GetSearchSuggestionsAsync(query);
                    if (!result.Succeeded)
                        return BadRequest(result);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while getting search suggestions");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            /// <summary>
            /// Get available search filters
            /// </summary>
            [HttpGet("filters")]
            [ProducesResponseType(typeof(ApiResponse<Dictionary<string, List<string>>>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<Dictionary<string, List<string>>>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<Dictionary<string, List<string>>>>> GetSearchFilters()
            {
                try
                {
                    var result = await _searchService.GetSearchFiltersAsync();
                    if (!result.Succeeded)
                        return BadRequest(result);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while getting search filters");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            /// <summary>
            /// Get search metadata
            /// </summary>
            [HttpGet("metadata")]
            [ProducesResponseType(typeof(ApiResponse<SearchMetadataDto>), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse<SearchMetadataDto>), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse<SearchMetadataDto>>> GetSearchMetadata()
            {
                try
                {
                    var result = await _searchService.GetSearchMetadataAsync();
                    if (!result.Succeeded)
                        return BadRequest(result);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while getting search metadata");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
