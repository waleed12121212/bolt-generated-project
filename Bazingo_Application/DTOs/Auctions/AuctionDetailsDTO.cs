using System;
using System.Collections.Generic;
using Bazingo_Core.Entities;
using Bazingo_Application.DTOs.Bids;
using Bazingo_Core.Enums;
using Bazingo_Core.Entities.Auction;

namespace Bazingo_Application.DTOs.Auctions
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
}
