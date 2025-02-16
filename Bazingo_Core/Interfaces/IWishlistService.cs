    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Bazingo_Core.Entities.Shopping;

    namespace Bazingo_Core.Interfaces
    {
        public interface IWishlistService
        {
            Task<WishlistEntity> GetWishlistByUserIdAsync(string userId);
            Task<WishlistEntity> AddToWishlistAsync(string userId, int productId);
            Task<bool> RemoveFromWishlistAsync(string userId, int wishlistItemId);
            Task<bool> ClearWishlistAsync(string userId);
            Task<bool> MoveToCartAsync(string userId, int wishlistItemId);
        }
    }
