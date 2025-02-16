using System.Collections.Generic;

namespace Bazingo_Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
        public virtual ICollection<Product.ProductEntity> Products { get; set; }

        public Category()
        {
            SubCategories = new HashSet<Category>();
            Products = new HashSet<Product.ProductEntity>();
            IsActive = true; // Default to active when created
        }
    }
}
