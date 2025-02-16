using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Bazingo_Core.Entities.Auction;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Enums;

namespace Bazingo_Core.DomainLogic
{
    public class AuctionManager : IAuctionManager
    {
        public class AuctionDetailsDTO
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductImage { get; set; }
            public decimal StartingPrice { get; set; }
            public decimal CurrentPrice { get; set; }
            public decimal MinimumBidIncrement { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public AuctionStatus Status { get; set; }
            public string SellerId { get; set; }
            public string SellerName { get; set; }
            public string WinnerName { get; set; }
            public int BidCount => Bids?.Count ?? 0;
            public bool HasEnded => DateTime.UtcNow >= EndTime;
            public List<BidDTO> Bids { get; set; } = new List<BidDTO>();
        }

        public class BidDTO
        {
            public int Id { get; set; }
            public int AuctionId { get; set; }
            public string BidderId { get; set; }
            public string BidderName { get; set; }
            public decimal Amount { get; set; }
            public DateTime BidTime { get; set; }
            public bool IsWinning { get; set; }
        }

        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public AuctionManager(IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
        }

        public async Task<AuctionDetailsDTO> GetAuctionByIdAsync(int id)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(id);
            if (auction == null)
                return null;

            var bids = await _bidRepository.GetBidsByAuctionIdAsync(id);

            return new AuctionDetailsDTO
            {
                Id = auction.Id,
                ProductId = auction.ProductId,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = auction.CurrentPrice,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                Status = auction.Status,
                SellerId = auction.SellerId,
                MinimumBidIncrement = auction.MinimumBidIncrement,
                Bids = bids?.Select(b => new BidDTO
                {
                    Id = b.Id,
                    AuctionId = b.AuctionId,
                    BidderId = b.BidderId,
                    Amount = b.Amount,
                    BidTime = b.BidTime,
                    IsWinning = b.IsWinning
                }).ToList()
            };
        }

        public async Task<AuctionEntity> CreateAuctionAsync(AuctionDetailsDTO auctionDetails)
        {
            var auction = new AuctionEntity
            {
                ProductId = auctionDetails.ProductId,
                SellerId = auctionDetails.SellerId,
                StartingPrice = auctionDetails.StartingPrice,
                MinimumBidIncrement = auctionDetails.MinimumBidIncrement,
                StartTime = auctionDetails.StartTime,
                EndTime = auctionDetails.EndTime,
                Status = AuctionStatus.Active
            };

            return await _auctionRepository.AddAsync(auction);
        }

        public async Task<bool> PlaceBidAsync(BidEntity bid)
        {
            return await _auctionRepository.PlaceBidAsync(bid);
        }

        public async Task<bool> EndAuctionAsync(int auctionId)
        {
            return await _auctionRepository.EndAuctionAsync(auctionId);
        }

        public async Task<BidEntity> GetWinningBidAsync(int auctionId)
        {
            return await _bidRepository.GetHighestBidForAuctionAsync(auctionId);
        }

        public async Task<IEnumerable<AuctionEntity>> GetActiveAuctionsAsync()
        {
            return await _auctionRepository.GetActiveAuctionsAsync();
        }

        public async Task<IEnumerable<AuctionEntity>> GetSellerAuctionsAsync(string sellerId)
        {
            return await _auctionRepository.GetSellerAuctionsAsync(sellerId);
        }

        public async Task<IEnumerable<AuctionEntity>> GetBidderAuctionsAsync(string bidderId)
        {
            return await _auctionRepository.GetBidderAuctionsAsync(bidderId);
        }

        public bool ValidateAuction(AuctionEntity auction)
        {
            return auction != null
                && auction.StartingPrice > 0
                && auction.StartTime < auction.EndTime;
        }
    }
}
