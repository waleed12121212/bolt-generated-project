    using Bazingo_Application.DTOs.Wishlist;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Models.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [Authorize]
        public class WishlistController : BaseController
        {
            private readonly IWishlistService _wishlistService;
            private readonly ILogger<WishlistController> _logger;

            public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger)
            {
                _wishlistService = wishlistService;
                _logger = logger;
            }

            [HttpGet]
            public async Task<ActionResult<ApiResponse<WishlistDto>>> GetWishlist()
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.GetUserWishlistAsync(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user wishlist");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("items")]
            public async Task<ActionResult<ApiResponse<WishlistDto>>> AddToWishlist([FromBody] AddToWishlistDto dto)
            {
                if (dto == null)
                {
                    return BadRequest("AddToWishlistDto object is required.");
                }

                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.AddToWishlistAsync(userId, dto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding item to wishlist");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpDelete("items/{productId}")]
            public async Task<ActionResult<ApiResponse<bool>>> RemoveFromWishlist(int productId)
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.RemoveFromWishlistAsync(userId, productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing item from wishlist");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpDelete]
            public async Task<ActionResult<ApiResponse<bool>>> ClearWishlist()
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.ClearWishlistAsync(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing wishlist");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("items/{productId}/move-to-cart")]
            public async Task<ActionResult<ApiResponse<bool>>> MoveToCart(int productId)
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.MoveToCartAsync(userId, productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error moving item to cart");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("move-all-to-cart")]
            public async Task<ActionResult<ApiResponse<bool>>> MoveAllToCart()
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.MoveAllToCartAsync(userId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error moving all items to cart");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("items/{productId}/is-in-wishlist")]
            public async Task<ActionResult<ApiResponse<bool>>> IsInWishlist(int productId)
            {
                try
                {
                    var userId = User.FindFirst("sub")?.Value;
                    return await _wishlistService.IsInWishlistAsync(userId, productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if item is in wishlist");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
