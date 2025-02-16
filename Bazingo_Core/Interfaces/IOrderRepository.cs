using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Enums;
using Bazingo_Core.Specifications;

namespace Bazingo_Core.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        Task<OrderEntity> GetByIdAsync(int id);
        Task<OrderEntity> GetByIdWithItemsAsync(int id);
        Task<OrderEntity> GetByIdWithItemsAndStatusHistoryAsync(int id);
        Task<IReadOnlyList<OrderEntity>> GetAllAsync();
        Task<IReadOnlyList<OrderEntity>> GetAllWithItemsAsync();
        Task<IReadOnlyList<OrderEntity>> GetByUserIdAsync(string userId);
        Task<IReadOnlyList<OrderEntity>> GetByUserIdWithItemsAsync(string userId);
        Task<IReadOnlyList<OrderEntity>> GetUserOrdersAsync(string userId, int page = 1, int pageSize = 10);
        Task<IReadOnlyList<OrderEntity>> GetOrdersByStatusAsync(OrderStatus status, int page = 1, int pageSize = 10);
        Task<OrderEntity> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus, string comment = null);
        Task<OrderEntity> UpdatePaymentStatusAsync(int orderId, PaymentStatus newStatus);

        // CRUD operations
        Task<OrderEntity> AddAsync(OrderEntity order);
        Task<OrderEntity> UpdateAsync(OrderEntity order);
        Task DeleteAsync(OrderEntity order);
        Task<OrderEntity> GetEntityWithSpec(ISpecification<OrderEntity> spec);
    }
}
