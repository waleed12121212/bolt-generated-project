using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Reviews
{
    public class ReviewCreateDTO
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required, Range(1 , 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
