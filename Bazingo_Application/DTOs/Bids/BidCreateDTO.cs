using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Bids
{
    public class BidCreateDTO
    {
        [Required]
        public int AuctionID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required, Range(0.01 , double.MaxValue)]
        public decimal BidAmount { get; set; }
    }
}
