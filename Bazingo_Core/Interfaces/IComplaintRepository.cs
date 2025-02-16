using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Interfaces
{
    public interface IComplaintRepository : IBaseRepository<Complaint>
    {
        Task<IReadOnlyList<Complaint>> GetComplaintsByOrderAsync(int orderId);
        Task<IReadOnlyList<Complaint>> GetComplaintsByUserAsync(string userId);
        Task<IReadOnlyList<Complaint>> GetComplaintsByProductAsync(int productId);
        Task<IReadOnlyList<Complaint>> GetComplaintsByStatusAsync(Bazingo_Core.Enums.ComplaintStatus status);

        // New methods to match the controller
        Task<Complaint> AddComplaintAsync(Complaint complaint);
        Task<Complaint> GetComplaintByIdAsync(int id);
        Task<IReadOnlyList<Complaint>> GetComplaintsByUserIdAsync(string userId);
        Task UpdateComplaintStatusAsync(int id, string status);
    }
}
