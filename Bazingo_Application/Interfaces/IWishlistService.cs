using Bazingo_Application.DTOs.Wishlist;
using Bazingo_Core.Models.Common;
using System.Threading.Tasks;

namespace Bazingo_Application.Interfaces
{
    public interface IWishlistService
    {
        Task<ApiResponse<WishlistDto>> GetUserWishlistAsync(string userId);
        Task<ApiResponse<WishlistDto>> AddToWishlistAsync(string userId, AddToWishlistDto dto);
        Task<ApiResponse<bool>> RemoveFromWishlistAsync(string userId, int productId);
        Task<ApiResponse<bool>> ClearWishlistAsync(string userId);
        Task<ApiResponse<bool>> MoveToCartAsync(string userId, int productId);
        Task<ApiResponse<bool>> MoveAllToCartAsync(string userId);
        Task<ApiResponse<bool>> IsInWishlistAsync(string userId, int productId);
    }
}
