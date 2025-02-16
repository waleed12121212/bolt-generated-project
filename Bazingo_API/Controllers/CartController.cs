using Bazingo_Application.DTOs.Cart;
using Bazingo_Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Get the current user's shopping cart
        /// </summary>
        /// <returns>The user's cart with all items</returns>
        /// <response code="200">Returns the user's cart</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet]
        [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        /// <summary>
        /// Add an item to the shopping cart
        /// </summary>
        /// <param name="addToCartDto">The item to add</param>
        /// <returns>The updated cart</returns>
        /// <response code="200">Returns the updated cart</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPost]
        [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<CartDto>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.AddToCartAsync(userId, addToCartDto);
            return Ok(cart);
        }

        /// <summary>
        /// Update an item in the shopping cart
        /// </summary>
        /// <param name="updateCartItemDto">The updated item details</param>
        /// <returns>The updated cart</returns>
        /// <response code="200">Returns the updated cart</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpPut("{itemId}")]
        [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<CartDto>> UpdateCartItem(int itemId, [FromBody] UpdateCartItemDto updateCartItemDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.UpdateCartItemAsync(userId, updateCartItemDto);
            return Ok(cart);
        }

        /// <summary>
        /// Remove an item from the shopping cart
        /// </summary>
        /// <param name="itemId">The ID of the item to remove</param>
        /// <returns>Success status</returns>
        /// <response code="200">If the item was removed successfully</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpDelete("{itemId}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<bool>> RemoveFromCart(int itemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveFromCartAsync(userId, itemId);
            return Ok(result);
        }

        /// <summary>
        /// Clear all items from the shopping cart
        /// </summary>
        /// <returns>Success status</returns>
        /// <response code="200">If the cart was cleared successfully</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<bool>> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }
    }
}
