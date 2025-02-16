using System;
using System.Collections.Generic;

namespace Bazingo_Application.DTOs.Cart
{
    /// <summary>
    /// Data transfer object for cart information
    /// </summary>
    public class CartDto
    {
        /// <summary>
        /// The unique identifier for the cart
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user who owns the cart
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The total amount of all items in the cart
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// The date and time when the cart was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Whether the cart has been deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The items in the cart
        /// </summary>
        public ICollection<CartItemDto> Items { get; set; }

        public CartDto()
        {
            Items = new List<CartItemDto>();
            LastUpdated = DateTime.UtcNow;
        }
    }
}
