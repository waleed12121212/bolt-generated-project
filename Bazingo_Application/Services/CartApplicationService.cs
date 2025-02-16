using Bazingo_Application.DTOs.Cart;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Application.Services
{
    public class CartApplicationService : Bazingo_Application.Interfaces.ICartService
    {
        private readonly Bazingo_Core.Interfaces.ICartService _cartService;
        private readonly ILogger<CartApplicationService> _logger;

        public CartApplicationService(Bazingo_Core.Interfaces.ICartService cartService, ILogger<CartApplicationService> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<ApiResponse<CartDto>> GetUserCartAsync(string userId)
        {
            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.CreateError("Cart not found");
                }

                var cartDto = new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    TotalAmount = cart.TotalAmount,
                    LastUpdated = cart.LastUpdated,
                    Items = cart.Items.Select(item => new CartItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.Product?.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                        Price = item.UnitPrice
                    }).ToList()
                };

                return ApiResponse<CartDto>.CreateSuccess(cartDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user {UserId}", userId);
                return ApiResponse<CartDto>.CreateError("Error retrieving cart");
            }
        }

        public async Task<ApiResponse<CartDto>> GetOrCreateCartAsync(string userId)
        {
            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    cart = await _cartService.AddToCartAsync(userId, 0, 0); // Create empty cart
                }

                var cartDto = new CartDto
                {
                    Id = cart.Id,
                    UserId = cart.UserId,
                    TotalAmount = cart.TotalAmount,
                    LastUpdated = cart.LastUpdated,
                    Items = cart.Items.Select(item => new CartItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.Product?.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,
                        Price = item.UnitPrice
                    }).ToList()
                };

                return ApiResponse<CartDto>.CreateSuccess(cartDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting or creating cart for user {UserId}", userId);
                return ApiResponse<CartDto>.CreateError("Error accessing cart");
            }
        }

        public async Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto)
        {
            try
            {
                var cart = await _cartService.AddToCartAsync(userId, dto.ProductId, dto.Quantity);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.CreateError("Failed to add item to cart");
                }

                return await GetUserCartAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                return ApiResponse<CartDto>.CreateError("Error adding item to cart");
            }
        }

        public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto)
        {
            try
            {
                var cart = await _cartService.UpdateCartItemQuantityAsync(userId, dto.CartItemId, dto.Quantity);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.CreateError("Failed to update cart item");
                }

                return await GetUserCartAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item for user {UserId}", userId);
                return ApiResponse<CartDto>.CreateError("Error updating cart item");
            }
        }

        public async Task<ApiResponse<bool>> RemoveFromCartAsync(string userId, int productId)
        {
            try
            {
                var success = await _cartService.RemoveFromCartAsync(userId, productId);
                return success 
                    ? ApiResponse<bool>.CreateSuccess(true, "Item removed from cart")
                    : ApiResponse<bool>.CreateError("Failed to remove item from cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart for user {UserId}", userId);
                return ApiResponse<bool>.CreateError("Error removing item from cart");
            }
        }

        public async Task<ApiResponse<bool>> ClearCartAsync(string userId)
        {
            try
            {
                var success = await _cartService.ClearCartAsync(userId);
                return success 
                    ? ApiResponse<bool>.CreateSuccess(true, "Cart cleared")
                    : ApiResponse<bool>.CreateError("Failed to clear cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                return ApiResponse<bool>.CreateError("Error clearing cart");
            }
        }
    }
}
