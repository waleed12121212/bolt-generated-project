using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Ecrows
{
    public class EscrowDTO
    {
        public int EscrowID { get; set; }
        public int OrderID { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // Held, Released, Refunded
        public DateTime CreatedAt { get; set; }
    }
}
