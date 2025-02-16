using System;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities.Auction
{
    public class BidEntity : BaseEntity
    {
        public int AuctionId { get; set; }
        public string BidderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public bool IsWinning { get; set; }

        // Navigation properties
        public virtual AuctionEntity Auction { get; set; }
        public virtual ApplicationUser Bidder { get; set; }

        public BidEntity()
        {
            BidTime = DateTime.UtcNow;
        }
    }
}
