    using Bazingo_Application.DTOs.Ecrows;
    using Bazingo_Core.Models;
    using Bazingo_Core.DomainLogic;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Core.Entities.Payment;
    using Bazingo_Core.Enums;
    using Bazingo_Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class EscrowController : ControllerBase
        {
            private readonly IEscrowRepository _escrowRepository;
            private readonly EscrowManager _escrowManager;
            private readonly ILogger<EscrowController> _logger;

            public EscrowController(IEscrowRepository escrowRepository, EscrowManager escrowManager, ILogger<EscrowController> logger)
            {
                _escrowRepository = escrowRepository;
                _escrowManager = escrowManager;
                _logger = logger;
            }

            // Create Escrow
            [HttpPost]
            public async Task<IActionResult> CreateEscrow([FromBody] EscrowCreateDTO escrowCreateDTO)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    var escrowTransaction = new EscrowTransaction
                    {
                        OrderId = escrowCreateDTO.OrderID,
                        Amount = escrowCreateDTO.Amount,
                        Status = Enum.Parse<EscrowStatus>(escrowCreateDTO.Status, true),
                        CreatedAt = DateTime.UtcNow
                    };

                    await _escrowRepository.AddEscrowAsync(escrowTransaction);
                    return Ok(new { message = "Escrow created successfully." });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating escrow");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Get Escrow by ID
            [HttpGet("{id}")]
            public async Task<IActionResult> GetEscrowById(int id)
            {
                try
                {
                    var escrowTransaction = await _escrowRepository.GetEscrowByIdAsync(id);
                    if (escrowTransaction == null) return NotFound(new { message = "Escrow not found." });

                    var escrowDTO = new EscrowDTO
                    {
                        EscrowID = escrowTransaction.Id,
                        OrderID = escrowTransaction.OrderId,
                        Amount = escrowTransaction.Amount,
                        Status = escrowTransaction.Status.ToString(),
                        CreatedAt = escrowTransaction.CreatedAt
                    };

                    return Ok(escrowDTO);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrow by ID: {EscrowId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Update Escrow Status
            [HttpPut("status/{id}")]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<IActionResult> UpdateEscrowStatus(int id, [FromBody] EscrowUpdateDTO escrowUpdateDTO)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    var escrowTransaction = await _escrowRepository.GetEscrowByIdAsync(id);
                    if (escrowTransaction == null) return NotFound(new { message = "Escrow not found." });

                    escrowTransaction.Status = Enum.Parse<EscrowStatus>(escrowUpdateDTO.Status, true);
                    await _escrowRepository.UpdateEscrowAsync(escrowTransaction);

                    return Ok(new { message = "Escrow status updated successfully." });
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating escrow status for ID: {EscrowId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }

            // Get All Escrows
            [HttpGet]
            [Authorize(Roles = Constants.Roles.Admin)]
            public async Task<IActionResult> GetAllEscrows()
            {
                try
                {
                    var escrowTransactions = await _escrowRepository.GetAllEscrowsAsync();
                    var escrowDTOs = escrowTransactions.Select(e => new EscrowDTO
                    {
                        EscrowID = e.Id,
                        OrderID = e.OrderId,
                        Amount = e.Amount,
                        Status = e.Status.ToString(),
                        CreatedAt = e.CreatedAt
                    });

                    return Ok(escrowDTOs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all escrows");
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
