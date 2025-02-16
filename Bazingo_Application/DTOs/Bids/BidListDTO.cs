using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Bids
{
    public class BidListDTO
    {
        public int AuctionID { get; set; }
        public List<BidDTO> Bids { get; set; }
    }
}
