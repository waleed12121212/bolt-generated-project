using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Cart
{
    /// <summary>
    /// Data transfer object for updating a cart item
    /// </summary>
    public sealed class UpdateCartItemDto
    {
        /// <summary>
        /// The ID of the cart item to update
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid cart item")]
        public int CartItemId { get; set; }

        /// <summary>
        /// The ID of the product to update in the cart
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product")]
        public int ProductId { get; set; }

        /// <summary>
        /// The new quantity for the product
        /// </summary>
        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }
    }
}
