using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Enums;

namespace Bazingo_Core.Entities.Product
{
    public class ProductEntity : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        public int ViewCount { get; set; }
        public double AverageRating { get; set; }
        public ProductStatus Status { get; set; }
        public string SellerId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        public virtual ApplicationUser Seller { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual ICollection<ProductReviewEntity> Reviews { get; set; }
        public virtual ICollection<ProductImageEntity> Images { get; set; }
        public virtual ICollection<ProductAttributeEntity> Attributes { get; set; }

        public ProductEntity()
        {
            Reviews = new HashSet<ProductReviewEntity>();
            Images = new HashSet<ProductImageEntity>();
            Attributes = new HashSet<ProductAttributeEntity>();
            IsAvailable = true;
            IsFeatured = false;
            ViewCount = 0;
            AverageRating = 0;
            Status = ProductStatus.Draft;
            IsActive = true;
        }
    }
}
