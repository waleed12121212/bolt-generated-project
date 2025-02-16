using System.Threading.Tasks;
using System.Collections.Generic;
using Bazingo_Core.Entities.Shopping;

namespace Bazingo_Core.Interfaces
{
    public interface IOrderService
    {
        Task<OrderEntity> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderEntity>> GetOrdersByUserIdAsync(string userId);
        Task<OrderEntity> CreateOrderAsync(string userId);
        Task<OrderEntity> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
