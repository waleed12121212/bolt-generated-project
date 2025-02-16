using System;
using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Core.Entities.Shopping
{
    public class WishlistItemEntity : BaseEntity
    {
        [Required]
        public int WishlistId { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        public DateTime AddedDate { get; set; }
        public string Notes { get; set; }
        public decimal? PriceWhenAdded { get; set; }

        // Navigation properties
        public virtual WishlistEntity Wishlist { get; set; }
        public virtual ProductEntity Product { get; set; }

        public WishlistItemEntity()
        {
            AddedDate = DateTime.UtcNow;
        }
    }
}
