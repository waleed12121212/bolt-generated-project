using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Bids
{
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
}
