using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Entities;
using Bazingo_Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Bazingo_Application.Interfaces;

namespace Bazingo_Application.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ComplaintService> _logger;

        public ComplaintService(IUnitOfWork unitOfWork, ILogger<ComplaintService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<Complaint>> GetComplaintByIdAsync(int complaintId)
        {
            try
            {
                var complaint = await _unitOfWork.Complaints.GetComplaintByIdAsync(complaintId);
                if (complaint == null)
                    return ApiResponse<Complaint>.CreateError("Complaint not found");

                return ApiResponse<Complaint>.CreateSuccess(complaint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting complaint with ID: {ComplaintId}", complaintId);
                return ApiResponse<Complaint>.CreateError("Error retrieving complaint");
            }
        }

        public async Task<ApiResponse<IReadOnlyList<Complaint>>> GetAllComplaintsAsync()
        {
            try
            {
                var complaints = await _unitOfWork.Complaints.GetAllAsync();
                return ApiResponse<IReadOnlyList<Complaint>>.CreateSuccess(complaints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all complaints");
                return ApiResponse<IReadOnlyList<Complaint>>.CreateError("Error retrieving complaints");
            }
        }

        public async Task<ApiResponse<IReadOnlyList<Complaint>>> GetComplaintsByUserIdAsync(string userId)
        {
            try
            {
                var complaints = await _unitOfWork.Complaints.GetComplaintsByUserIdAsync(userId);
                return ApiResponse<IReadOnlyList<Complaint>>.CreateSuccess(complaints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting complaints for user ID: {UserId}", userId);
                return ApiResponse<IReadOnlyList<Complaint>>.CreateError("Error retrieving user complaints");
            }
        }

        public async Task<ApiResponse<Complaint>> AddComplaintAsync(Complaint complaint)
        {
            try
            {
                var result = await _unitOfWork.Complaints.AddComplaintAsync(complaint);
                await _unitOfWork.CompleteAsync();
                return ApiResponse<Complaint>.CreateSuccess(result, "Complaint added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding complaint");
                return ApiResponse<Complaint>.CreateError("Error adding complaint");
            }
        }

        public async Task<ApiResponse<bool>> UpdateComplaintStatusAsync(int complaintId, string status)
        {
            try
            {
                var complaint = await _unitOfWork.Complaints.GetComplaintByIdAsync(complaintId);
                if (complaint == null)
                    return ApiResponse<bool>.CreateError("Complaint not found");

                if (!Enum.TryParse<Bazingo_Core.Enums.ComplaintStatus>(status, true, out var complaintStatus))
                    return ApiResponse<bool>.CreateError("Invalid complaint status");

                await _unitOfWork.Complaints.UpdateComplaintStatusAsync(complaintId, status);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<bool>.CreateSuccess(true, "Complaint status updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating complaint status for ID: {ComplaintId}", complaintId);
                return ApiResponse<bool>.CreateError("Error updating complaint status");
            }
        }
    }
}
