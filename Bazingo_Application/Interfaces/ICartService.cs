using Bazingo_Application.DTOs.Cart;
using Bazingo_Core.Models.Common;
using System.Threading.Tasks;

namespace Bazingo_Application.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetUserCartAsync(string userId);
        Task<ApiResponse<CartDto>> GetOrCreateCartAsync(string userId);
        Task<ApiResponse<CartDto>> AddToCartAsync(string userId, AddToCartDto dto);
        Task<ApiResponse<CartDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto);
        Task<ApiResponse<bool>> RemoveFromCartAsync(string userId, int productId);
        Task<ApiResponse<bool>> ClearCartAsync(string userId);
    }
}
