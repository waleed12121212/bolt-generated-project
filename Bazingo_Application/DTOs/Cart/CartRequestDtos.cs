using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Cart
{
    public sealed class CartItemRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive number")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
        public int Quantity { get; set; }
    }
}
