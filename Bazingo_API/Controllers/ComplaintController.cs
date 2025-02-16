using Bazingo_Application.Services;
using Bazingo_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bazingo_Application.DTOs.Complaints;
using Bazingo_Core.Entities;
using Bazingo_Core.Interfaces;

namespace Bazingo_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintRepository _complaintRepository;

        public ComplaintController(IComplaintRepository complaintRepository)
        {
            _complaintRepository = complaintRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint([FromBody] Complaint complaint)
        {
            await _complaintRepository.AddComplaintAsync(complaint);
            return CreatedAtAction(nameof(GetComplaintById) , new { id = complaint.Id } , complaint);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint = await _complaintRepository.GetComplaintByIdAsync(id);
            if (complaint == null) return NotFound();
            return Ok(complaint);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserComplaints(string userId)
        {
            var complaints = await _complaintRepository.GetComplaintsByUserIdAsync(userId);
            return Ok(complaints);
        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateComplaintStatus(int id , [FromBody] string status)
        {
            await _complaintRepository.UpdateComplaintStatusAsync(id , status);
            return Ok(new { message = "Complaint status updated successfully." });
        }
    }
}
