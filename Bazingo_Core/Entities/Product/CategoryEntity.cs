using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bazingo_Core.Entities.Product
{
    public class CategoryEntity : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        // Navigation properties
        public virtual CategoryEntity ParentCategory { get; set; }
        public virtual ICollection<CategoryEntity> SubCategories { get; set; }
        public virtual ICollection<ProductEntity> Products { get; set; }

        public CategoryEntity()
        {
            SubCategories = new HashSet<CategoryEntity>();
            Products = new HashSet<ProductEntity>();
        }
    }
}
