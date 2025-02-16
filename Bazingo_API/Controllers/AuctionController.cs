    using Bazingo_Core.DomainLogic;
    using Bazingo_Core.Entities;
    using Bazingo_Core.Entities.Auction;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Bazingo_Application.DTOs.Auctions;
    using Bazingo_Application.DTOs.Bids;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Bazingo_Core.Interfaces;
    using Bazingo_Core.Enums;
    using static Bazingo_Core.DomainLogic.AuctionManager;
    using Bazingo_Core;

    namespace Bazingo_API.Controllers
    {
        [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class AuctionController : ControllerBase
        {
            private readonly IAuctionManager _auctionManager;
            private readonly ILogger<AuctionController> _logger;

            public AuctionController(IAuctionManager auctionManager, ILogger<AuctionController> logger)
            {
                _auctionManager = auctionManager;
                _logger = logger;
            }

            [HttpPost]
            [Authorize(Roles = Constants.Roles.Seller)]
            public async Task<IActionResult> CreateAuction([FromBody] AuctionCreateDTO auctionDTO)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    // Create AuctionManager.AuctionDetailsDTO
                    var auctionDetails = new AuctionManager.AuctionDetailsDTO
                    {
                        ProductId = auctionDTO.ProductID,
                        StartingPrice = auctionDTO.StartPrice,
                        EndTime = auctionDTO.EndTime,
                        StartTime = DateTime.UtcNow,
                        MinimumBidIncrement = 1.0m,
                        SellerId = User.Identity.Name,
                        Status = AuctionStatus.Active
                    };

                    var result = await _auctionManager.CreateAuctionAsync(auctionDetails);
                    return result != null
                        ? Ok(new { message = "Auction created successfully", auctionId = result.Id })
                        : BadRequest(new { message = "Failed to create auction" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating auction");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpPost("bid")]
            public async Task<IActionResult> PlaceBid([FromBody] BidCreateDTO bidDTO)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    var bid = new BidEntity
                    {
                        AuctionId = bidDTO.AuctionID,
                        BidderId = User.Identity.Name,
                        Amount = bidDTO.BidAmount,
                        BidTime = DateTime.UtcNow,
                        IsWinning = false,
                        CreatedAt = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    var result = await _auctionManager.PlaceBidAsync(bid);
                    return result ? Ok(new { message = "Bid placed successfully" }) : BadRequest(new { message = "Failed to place bid" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error placing bid");
                    return StatusCode(500, "Internal Server Error");
                }
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetAuctionDetails(int id)
            {
                try
                {
                    var auction = await _auctionManager.GetAuctionByIdAsync(id);
                    if (auction == null)
                        return NotFound();

                    return Ok(auction);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting auction details for ID: {AuctionId}", id);
                    return StatusCode(500, "Internal Server Error");
                }
            }
        }
    }
