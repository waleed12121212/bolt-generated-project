using System.Threading.Tasks;
using System.Collections.Generic;
using Bazingo_Core.Entities.Shopping;

namespace Bazingo_Core.Interfaces
{
    public interface ICartService
    {
        Task<CartEntity> GetCartByUserIdAsync(string userId);
        Task<CartEntity> AddToCartAsync(string userId, int productId, int quantity);
        Task<CartEntity> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity);
        Task<bool> RemoveFromCartAsync(string userId, int cartItemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
