using System;
using System.Collections.Generic;
using Bazingo_Application.DTOs.Product;

namespace Bazingo_Application.DTOs.Wishlist
{
    public class WishlistDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<WishlistItemDto> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
