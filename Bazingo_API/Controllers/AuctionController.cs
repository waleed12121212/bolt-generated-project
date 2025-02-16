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

namespace Bazingo_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionManager _auctionManager;

        public AuctionController(IAuctionManager auctionManager)
        {
            _auctionManager = auctionManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionCreateDTO auctionDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create AuctionManager.AuctionDetailsDTO
            var auctionDetails = new AuctionManager.AuctionDetailsDTO
            {
                ProductId = auctionDTO.ProductID ,
                StartingPrice = auctionDTO.StartPrice ,
                EndTime = auctionDTO.EndTime ,
                StartTime = DateTime.UtcNow ,
                MinimumBidIncrement = 1.0m ,
                SellerId = User.Identity.Name ,
                Status = AuctionStatus.Active
            };

            var result = await _auctionManager.CreateAuctionAsync(auctionDetails);
            return result != null
                ? Ok(new { message = "Auction created successfully" , auctionId = result.Id })
                : BadRequest(new { message = "Failed to create auction" });
        }

        [HttpPost("bid")]
        public async Task<IActionResult> PlaceBid([FromBody] BidCreateDTO bidDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var bid = new BidEntity
            {
                AuctionId = bidDTO.AuctionID ,
                BidderId = User.Identity.Name ,
                Amount = bidDTO.BidAmount ,
                BidTime = DateTime.UtcNow ,
                IsWinning = false ,
                CreatedAt = DateTime.UtcNow ,
                LastUpdated = DateTime.UtcNow ,
                IsDeleted = false
            };

            var result = await _auctionManager.PlaceBidAsync(bid);
            return result ? Ok(new { message = "Bid placed successfully" }) : BadRequest(new { message = "Failed to place bid" });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionDetails(int id)
        {
            var auction = await _auctionManager.GetAuctionByIdAsync(id);
            if (auction == null)
                return NotFound();

            return Ok(auction);
        }
    }
}
