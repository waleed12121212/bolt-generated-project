using System;
using Bazingo_Core.Entities.Identity;

namespace Bazingo_Core.Entities.Review
{
    public class ProductReview : BaseEntity
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Helpful { get; set; }

        // Navigation properties
        public virtual Product.ProductEntity Product { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ProductReview( )
        {
            ReviewDate = DateTime.UtcNow;
            IsVerifiedPurchase = false;
            Helpful = 0;
        }
    }
}
