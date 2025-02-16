using System.ComponentModel.DataAnnotations;

namespace Bazingo_Core.Entities.Product
{
    public class ProductImageEntity : BaseEntity
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string AltText { get; set; }

        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation properties
        public virtual ProductEntity Product { get; set; }
    }
}
