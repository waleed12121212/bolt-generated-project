using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Product;
using Bazingo_Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bazingo_Core.Entities.Auction
{
    public class AuctionEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public string SellerId { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal MinimumBidIncrement { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AuctionStatus Status { get; set; }
        public string WinnerId { get; set; }

        // Navigation properties
        public virtual ProductEntity Product { get; set; }
        public virtual ApplicationUser Seller { get; set; }
        public virtual ApplicationUser Winner { get; set; }
        public virtual ICollection<BidEntity> Bids { get; set; }

        // Computed properties
        public decimal CurrentPrice => Bids.Any() ? Bids.Max(b => b.Amount) : StartingPrice;

        public AuctionEntity()
        {
            Bids = new HashSet<BidEntity>();
        }
    }
}
