    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Shopping;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class WishlistRepository : BaseRepository<WishlistEntity>, IWishlistRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly DbSet<WishlistEntity> _wishlistSet;
            private readonly DbSet<WishlistItemEntity> _wishlistItemSet;
            private readonly ILogger<WishlistRepository> _logger;

            public WishlistRepository(ApplicationDbContext context, ILogger<WishlistRepository> logger) : base(context)
            {
                _context = context;
                _wishlistSet = context.Set<WishlistEntity>();
                _wishlistItemSet = context.Set<WishlistItemEntity>();
                _logger = logger;
            }

            public async Task<WishlistEntity> GetWishlistByUserIdAsync(string userId)
            {
                try
                {
                    return await _wishlistSet
                        .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting wishlist by user ID: {UserId}", userId);
                    return null;
                }
            }

            public async Task<WishlistEntity> GetWishlistWithItemsAsync(string userId)
            {
                try
                {
                    return await _wishlistSet
                        .Include(w => w.Items)
                        .ThenInclude(wi => wi.Product)
                        .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting wishlist with items for user ID: {UserId}", userId);
                    return null;
                }
            }

            public async Task<WishlistItemEntity> GetWishlistItemAsync(int wishlistId, int productId)
            {
                try
                {
                    return await _wishlistItemSet
                        .FirstOrDefaultAsync(wi => wi.WishlistId == wishlistId && wi.ProductId == productId && !wi.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting wishlist item for wishlist ID: {WishlistId} and product ID: {ProductId}", wishlistId, productId);
                    return null;
                }
            }

            public async Task<bool> AddItemToWishlistAsync(WishlistItemEntity wishlistItem)
            {
                try
                {
                    await _wishlistItemSet.AddAsync(wishlistItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding item to wishlist for wishlist ID: {WishlistId} and product ID: {ProductId}", wishlistItem.WishlistId, wishlistItem.ProductId);
                    return false;
                }
            }

            public async Task<bool> RemoveItemFromWishlistAsync(int wishlistId, int productId)
            {
                try
                {
                    var item = await GetWishlistItemAsync(wishlistId, productId);
                    if (item == null)
                        return false;

                    item.IsDeleted = true;
                    item.LastUpdated = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing item from wishlist for wishlist ID: {WishlistId} and product ID: {ProductId}", wishlistId, productId);
                    return false;
                }
            }

            public async Task<bool> ClearWishlistAsync(int wishlistId)
            {
                try
                {
                    var items = await _wishlistItemSet
                        .Where(wi => wi.WishlistId == wishlistId && !wi.IsDeleted)
                        .ToListAsync();

                    foreach (var item in items)
                    {
                        item.IsDeleted = true;
                        item.LastUpdated = DateTime.UtcNow;
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing wishlist with ID: {WishlistId}", wishlistId);
                    return false;
                }
            }

            public async Task<int> GetWishlistCountAsync(string userId)
            {
                try
                {
                    var wishlist = await GetWishlistByUserIdAsync(userId);
                    if (wishlist == null)
                        return 0;

                    return await _wishlistItemSet
                        .CountAsync(wi => wi.WishlistId == wishlist.Id && !wi.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting wishlist count for user ID: {UserId}", userId);
                    return 0;
                }
            }

            public async Task<bool> MoveItemToCartAsync(WishlistItemEntity wishlistItem, CartEntity cart)
            {
                try
                {
                    // Create new cart item
                    var cartItem = new CartItemEntity
                    {
                        CartId = cart.Id,
                        ProductId = wishlistItem.ProductId,
                        Quantity = 1,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    // Add to cart
                    await _context.CartItems.AddAsync(cartItem);

                    // Remove from wishlist
                    wishlistItem.IsDeleted = true;
                    wishlistItem.LastUpdated = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error moving item from wishlist to cart for wishlist item ID: {WishlistItemId} and cart ID: {CartId}", wishlistItem.Id, cart.Id);
                    return false;
                }
            }

            public async Task<bool> IsProductInWishlistAsync(string userId, int productId)
            {
                try
                {
                    var wishlist = await GetWishlistByUserIdAsync(userId);
                    if (wishlist == null)
                        return false;

                    return await _wishlistItemSet
                        .AnyAsync(wi => wi.WishlistId == wishlist.Id && wi.ProductId == productId && !wi.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if product {ProductId} is in wishlist for user {UserId}", productId, userId);
                    return false;
                }
            }
        }
    }
