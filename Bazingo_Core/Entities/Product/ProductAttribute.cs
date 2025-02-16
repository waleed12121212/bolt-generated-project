using System;
using Bazingo_Core.Entities.Product;

namespace Bazingo_Core.Entities.Product
{
    public class ProductAttribute : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public virtual ProductEntity Product { get; set; }
    }
}
