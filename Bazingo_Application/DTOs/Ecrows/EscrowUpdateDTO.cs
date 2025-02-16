using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Ecrows
{
    public class EscrowUpdateDTO
    {
        [Required]
        public int EscrowID { get; set; }

        [Required]
        public string Status { get; set; } // Held, Released, Refunded
    }
}
