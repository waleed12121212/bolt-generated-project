using Bazingo_Application.DTOs.Wishlist;
using Bazingo_Application.Interfaces;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Application.Services
{
    public class WishlistApplicationService : Bazingo_Application.Interfaces.IWishlistService
    {
        private readonly Bazingo_Core.Interfaces.IWishlistService _wishlistService;
        private readonly ILogger<WishlistApplicationService> _logger;

        public WishlistApplicationService(Bazingo_Core.Interfaces.IWishlistService wishlistService, ILogger<WishlistApplicationService> logger)
        {
            _wishlistService = wishlistService;
            _logger = logger;
        }

        public async Task<ApiResponse<WishlistDto>> GetUserWishlistAsync(string userId)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
                if (wishlist == null)
                {
                    return ApiResponse<WishlistDto>.CreateError("Wishlist not found");
                }

                var dto = new WishlistDto
                {
                    Id = wishlist.Id,
                    UserId = wishlist.UserId,
                    Items = wishlist.Items.Select(item => new WishlistItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Name = item.Product?.Name,
                        Price = item.Product?.Price ?? 0
                    }).ToList(),
                    CreatedAt = wishlist.CreatedAt,
                    UpdatedAt = wishlist.LastUpdated
                };

                return ApiResponse<WishlistDto>.CreateSuccess(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user wishlist");
                return ApiResponse<WishlistDto>.CreateError("Error retrieving wishlist");
            }
        }

        public async Task<ApiResponse<WishlistDto>> AddToWishlistAsync(string userId, AddToWishlistDto dto)
        {
            try
            {
                var result = await _wishlistService.AddToWishlistAsync(userId, dto.ProductId);
                if (result == null)
                {
                    return ApiResponse<WishlistDto>.CreateError("Failed to add item to wishlist");
                }

                return await GetUserWishlistAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to wishlist");
                return ApiResponse<WishlistDto>.CreateError("Error adding item to wishlist");
            }
        }

        public async Task<ApiResponse<bool>> RemoveFromWishlistAsync(string userId, int productId)
        {
            try
            {
                var result = await _wishlistService.RemoveFromWishlistAsync(userId, productId);
                return ApiResponse<bool>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from wishlist");
                return ApiResponse<bool>.CreateError("Error removing item from wishlist");
            }
        }

        public async Task<ApiResponse<bool>> ClearWishlistAsync(string userId)
        {
            try
            {
                var result = await _wishlistService.ClearWishlistAsync(userId);
                return ApiResponse<bool>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist");
                return ApiResponse<bool>.CreateError("Error clearing wishlist");
            }
        }

        public async Task<ApiResponse<bool>> MoveToCartAsync(string userId, int productId)
        {
            try
            {
                var result = await _wishlistService.MoveToCartAsync(userId, productId);
                return ApiResponse<bool>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error moving item to cart");
                return ApiResponse<bool>.CreateError("Error moving item to cart");
            }
        }

        public async Task<ApiResponse<bool>> MoveAllToCartAsync(string userId)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
                if (wishlist == null || !wishlist.Items.Any())
                {
                    return ApiResponse<bool>.CreateSuccess(true);
                }

                foreach (var item in wishlist.Items.ToList())
                {
                    await _wishlistService.MoveToCartAsync(userId, item.ProductId);
                }

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error moving all items to cart");
                return ApiResponse<bool>.CreateError("Error moving all items to cart");
            }
        }

        public async Task<ApiResponse<bool>> IsInWishlistAsync(string userId, int productId)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
                var isInWishlist = wishlist?.Items.Any(i => i.ProductId == productId) ?? false;
                return ApiResponse<bool>.CreateSuccess(isInWishlist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if item is in wishlist");
                return ApiResponse<bool>.CreateError("Error checking wishlist status");
            }
        }
    }
}
