using Bazingo_Core.Entities.Shopping;
using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface IWishlistRepository : IBaseRepository<WishlistEntity>
    {
        Task<WishlistEntity> GetWishlistByUserIdAsync(string userId);
        Task<WishlistEntity> GetWishlistWithItemsAsync(string userId);
        Task<WishlistItemEntity> GetWishlistItemAsync(int wishlistId, int productId);
        Task<bool> AddItemToWishlistAsync(WishlistItemEntity wishlistItem);
        Task<bool> RemoveItemFromWishlistAsync(int wishlistId, int productId);
        Task<bool> ClearWishlistAsync(int wishlistId);
        Task<int> GetWishlistCountAsync(string userId);
        Task<bool> MoveItemToCartAsync(WishlistItemEntity wishlistItem, CartEntity cart);
        Task<bool> IsProductInWishlistAsync(string userId, int productId);
    }
}
