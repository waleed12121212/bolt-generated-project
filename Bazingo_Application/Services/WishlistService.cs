    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using Bazingo_Application.Interfaces;
    using Bazingo_Application.DTOs.Wishlist;
    using Bazingo_Core.Models.Common;
    using Bazingo_Core.Entities.Shopping;
    using Bazingo_Core.Interfaces;
    using IWishlistServiceApp = Bazingo_Application.Interfaces.IWishlistService;

    namespace Bazingo_Application.Services
    {
        public class WishlistService : IWishlistServiceApp
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<WishlistService> _logger;

            public WishlistService(IUnitOfWork unitOfWork, ILogger<WishlistService> logger)
            {
                _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public async Task<ApiResponse<WishlistDto>> GetUserWishlistAsync(string userId)
            {
                try
                {
                    var wishlist = await _unitOfWork.Wishlists.GetWishlistWithItemsAsync(userId);
                    if (wishlist == null)
                    {
                        return ApiResponse<WishlistDto>.CreateSuccess(new WishlistDto());
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
                if (dto == null)
                {
                    return ApiResponse<WishlistDto>.CreateError("AddToWishlistDto object is required.");
                }

                try
                {
                    var result = await _unitOfWork.Wishlists.AddToWishlistAsync(userId, dto.ProductId);
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
                    var result = await _unitOfWork.Wishlists.RemoveItemFromWishlistAsync(userId, productId);
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
                    var result = await _unitOfWork.Wishlists.ClearWishlistAsync(userId);
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
                    var wishlist = await _unitOfWork.Wishlists.GetWishlistWithItemsAsync(userId);
                    if (wishlist == null)
                        return ApiResponse<bool>.CreateSuccess(false);

                    var item = wishlist.Items.FirstOrDefault(i => i.ProductId == productId);
                    if (item == null)
                        return ApiResponse<bool>.CreateSuccess(false);

                    var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
                    if (cart == null)
                    {
                        cart = new CartEntity { UserId = userId };
                        await _unitOfWork.Carts.AddAsync(cart);
                        await _unitOfWork.CompleteAsync();
                    }

                    await _unitOfWork.Wishlists.MoveItemToCartAsync(item, cart);
                    await _unitOfWork.CompleteAsync();

                    return ApiResponse<bool>.CreateSuccess(true);
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
                    var wishlist = await _unitOfWork.Wishlists.GetWishlistWithItemsAsync(userId);
                    if (wishlist == null || !wishlist.Items.Any())
                        return ApiResponse<bool>.CreateSuccess(true);

                    var cart = await _unitOfWork.Carts.GetCartWithItemsAsync(userId);
                    if (cart == null)
                    {
                        cart = new CartEntity { UserId = userId };
                        await _unitOfWork.Carts.AddAsync(cart);
                        await _unitOfWork.CompleteAsync();
                    }

                    foreach (var item in wishlist.Items.ToList())
                    {
                        await _unitOfWork.Wishlists.MoveItemToCartAsync(item, cart);
                    }

                    await _unitOfWork.CompleteAsync();
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
                    var isInWishlist = await _unitOfWork.Wishlists.IsProductInWishlistAsync(userId, productId);
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
