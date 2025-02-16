using Bazingo_Application.DTOs.Wishlist;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.GetUserWishlistAsync(userId);
        }

        [HttpPost("items")]
        public async Task<ActionResult<ApiResponse<WishlistDto>>> AddToWishlist([FromBody] AddToWishlistDto dto)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.AddToWishlistAsync(userId, dto);
        }

        [HttpDelete("items/{productId}")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveFromWishlist(int productId)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.RemoveFromWishlistAsync(userId, productId);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> ClearWishlist()
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.ClearWishlistAsync(userId);
        }

        [HttpPost("items/{productId}/move-to-cart")]
        public async Task<ActionResult<ApiResponse<bool>>> MoveToCart(int productId)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.MoveToCartAsync(userId, productId);
        }

        [HttpPost("move-all-to-cart")]
        public async Task<ActionResult<ApiResponse<bool>>> MoveAllToCart()
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.MoveAllToCartAsync(userId);
        }

        [HttpGet("items/{productId}/is-in-wishlist")]
        public async Task<ActionResult<ApiResponse<bool>>> IsInWishlist(int productId)
        {
            var userId = User.FindFirst("sub")?.Value;
            return await _wishlistService.IsInWishlistAsync(userId, productId);
        }
    }
}
