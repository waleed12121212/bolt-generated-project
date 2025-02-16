using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Cart
{
    /// <summary>
    /// Data transfer object for adding an item to the cart
    /// </summary>
    public sealed class AddToCartDto
    {
        /// <summary>
        /// The ID of the product to add to the cart
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product")]
        public int ProductId { get; set; }

        /// <summary>
        /// The quantity of the product to add
        /// </summary>
        [Required]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10")]
        public int Quantity { get; set; }
    }
}
