using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Enums;

namespace Bazingo_Application.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public string Note { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }
}
