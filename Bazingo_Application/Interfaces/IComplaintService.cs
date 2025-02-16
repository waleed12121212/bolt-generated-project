using Bazingo_Core.Entities;
using Bazingo_Core.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Application.Interfaces
{
    public interface IComplaintService
    {
        Task<ApiResponse<Complaint>> GetComplaintByIdAsync(int complaintId);
        Task<ApiResponse<IReadOnlyList<Complaint>>> GetAllComplaintsAsync();
        Task<ApiResponse<IReadOnlyList<Complaint>>> GetComplaintsByUserIdAsync(string userId);
        Task<ApiResponse<Complaint>> AddComplaintAsync(Complaint complaint);
        Task<ApiResponse<bool>> UpdateComplaintStatusAsync(int complaintId, string status);
    }
}
