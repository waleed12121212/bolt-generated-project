using Bazingo_Core.Entities.Shopping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface ICartRepository
    {
        Task<CartEntity> GetCartByUserIdAsync(string userId);
        Task<CartEntity> GetCartWithItemsAsync(string userId);
        Task<CartEntity> AddAsync(CartEntity cart);
        Task<CartItemEntity> GetCartItemAsync(int cartItemId);
        Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(string userId);
        Task<CartItemEntity> AddCartItemAsync(CartItemEntity cartItem);
        Task<bool> UpdateCartItemAsync(CartItemEntity cartItem);
        Task<bool> RemoveCartItemAsync(int cartItemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
