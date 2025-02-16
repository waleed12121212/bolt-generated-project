using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Ecrows
{
    public class EscrowCreateDTO
    {
        [Required]
        public int OrderID { get; set; }

        [Required, Range(0.01 , double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Status { get; set; } // Held, Released, Refunded
    }
}
