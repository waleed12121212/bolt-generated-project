using Bazingo_Core.Interfaces;
using Bazingo_Core.Entities.Shopping;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bazingo_Application.Services.Core
{
    public class CartCoreService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartCoreService> _logger;

        public CartCoreService(IUnitOfWork unitOfWork, ILogger<CartCoreService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CartEntity> GetCartByUserIdAsync(string userId)
        {
            try
            {
                return await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user {UserId}", userId);
                throw;
            }
        }

        public async Task<CartEntity> AddToCartAsync(string userId, int productId, int quantity)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    cart = new CartEntity { UserId = userId };
                    await _unitOfWork.Carts.AddAsync(cart);
                    await _unitOfWork.CompleteAsync();
                }

                // Check if the product already exists in the cart
                var existingCartItem = cart.Items.FirstOrDefault(ci => ci.ProductId == productId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    var cartItem = new CartItemEntity
                    {
                        CartId = cart.Id,
                        ProductId = productId,
                        Quantity = quantity
                    };
                    cart.Items.Add(cartItem);
                }

                await _unitOfWork.CompleteAsync();
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", userId);
                throw;
            }
        }

        public async Task<CartEntity> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart == null) return null;

                var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem == null) return null;

                cartItem.Quantity = quantity;
                await _unitOfWork.CompleteAsync();

                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item quantity for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart == null) return false;

                var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem == null) return false;

                cart.Items.Remove(cartItem);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart == null) return false;

                cart.Items.Clear();
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", userId);
                throw;
            }
        }
    }
}
