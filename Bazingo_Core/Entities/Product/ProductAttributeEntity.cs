using System.ComponentModel.DataAnnotations;

namespace Bazingo_Core.Entities.Product
{
    public class ProductAttributeEntity : BaseEntity
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Value { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        // Navigation properties
        public virtual ProductEntity Product { get; set; }
    }
}
