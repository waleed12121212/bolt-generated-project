    using Bazingo_Application.Services;
    using Bazingo_Core.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Application.DTOs.Complaints;
    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class ComplaintController : ControllerBase
        {
            private readonly IComplaintRepository _complaintRepository;
            private readonly ILogger<ComplaintController> _logger;

            public ComplaintController(IComplaintRepository complaintRepository, ILogger<ComplaintController> logger)
            {
                _complaintRepository = complaintRepository;
                _logger = logger;
            }

            [HttpPost]
            public async Task<IActionResult> CreateComplaint([FromBody] Complaint complaint)
            {
                if (complaint == null)
                {
                    return BadRequest("Complaint object is required.");
                }

                try
                {
                    await _complaintRepository.AddComplaintAsync(complaint);
                    return CreatedAtAction(nameof(GetComplaintById), new { id = complaint.Id }, complaint);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating complaint");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetComplaintById(int id)
            {
                try
                {
                    var complaint = await _complaintRepository.GetComplaintByIdAsync(id);
                    if (complaint == null) return NotFound();
                    return Ok(complaint);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaint by ID: {ComplaintId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("user/{userId}")]
            public async Task<IActionResult> GetUserComplaints(string userId)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is required.");
                }

                try
                {
                    var complaints = await _complaintRepository.GetComplaintsByUserIdAsync(userId);
                    return Ok(complaints);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting user complaints for user ID: {UserId}", userId);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPut("status/{id}")]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<IActionResult> UpdateComplaintStatus(int id, [FromBody] string status)
            {
                if (string.IsNullOrEmpty(status))
                {
                    return BadRequest("Status is required.");
                }

                try
                {
                    await _complaintRepository.UpdateComplaintStatusAsync(id, status);
                    return Ok(new { message = "Complaint status updated successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating complaint status for ID: {ComplaintId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
