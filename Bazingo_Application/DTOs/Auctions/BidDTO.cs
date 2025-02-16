using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Auctions
{
    public class BidDTO
    {
        public int BidID { get; set; }
        public decimal BidAmount { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
