using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Bids
{
    public class BidUpdateDTO
    {
        [Required]
        public int BidID { get; set; }

        [Required]
        public decimal NewBidAmount { get; set; }
    }
}
