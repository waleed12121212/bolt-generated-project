using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Shopping;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Application.Services.Core
{
    public class WishlistCoreService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WishlistCoreService> _logger;

        public WishlistCoreService(IUnitOfWork unitOfWork, ILogger<WishlistCoreService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WishlistEntity> GetWishlistByUserIdAsync(string userId)
        {
            try
            {
                return await _unitOfWork.Wishlists.GetWishlistWithItemsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting wishlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<WishlistEntity> AddToWishlistAsync(string userId, int productId)
        {
            try
            {
                var wishlist = await GetWishlistByUserIdAsync(userId);
                if (wishlist == null)
                {
                    wishlist = new WishlistEntity { UserId = userId };
                    await _unitOfWork.Wishlists.AddAsync(wishlist);
                    await _unitOfWork.CompleteAsync();
                }

                var wishlistItem = new WishlistItemEntity
                {
                    WishlistId = wishlist.Id,
                    ProductId = productId
                };

                wishlist.Items.Add(wishlistItem);
                await _unitOfWork.CompleteAsync();

                return wishlist;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to wishlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> RemoveFromWishlistAsync(string userId, int wishlistItemId)
        {
            try
            {
                var wishlist = await GetWishlistByUserIdAsync(userId);
                if (wishlist == null) return false;

                var wishlistItem = wishlist.Items.FirstOrDefault(wi => wi.Id == wishlistItemId);
                if (wishlistItem == null) return false;

                wishlist.Items.Remove(wishlistItem);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from wishlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ClearWishlistAsync(string userId)
        {
            try
            {
                var wishlist = await GetWishlistByUserIdAsync(userId);
                if (wishlist == null) return false;

                wishlist.Items.Clear();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing wishlist for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> MoveToCartAsync(string userId, int wishlistItemId)
        {
            try
            {
                var wishlist = await GetWishlistByUserIdAsync(userId);
                if (wishlist == null) return false;

                var wishlistItem = wishlist.Items.FirstOrDefault(wi => wi.Id == wishlistItemId);
                if (wishlistItem == null) return false;

                // Create cart item
                var cartItem = new CartItemEntity
                {
                    ProductId = wishlistItem.ProductId,
                    Quantity = 1
                };

                // Get or create cart
                var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
                if (cart == null)
                {
                    cart = new CartEntity { UserId = userId };
                    await _unitOfWork.Carts.AddAsync(cart);
                    await _unitOfWork.CompleteAsync();
                }

                cart.Items.Add(cartItem);

                // Remove from wishlist
                wishlist.Items.Remove(wishlistItem);

                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error moving item from wishlist to cart for user {UserId}", userId);
                throw;
            }
        }
    }
}
