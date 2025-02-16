using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Auctions
{
    public class AuctionCreateDTO
    {
        public int ProductID { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime EndTime { get; set; }
    }
}
