namespace Bazingo_Core.Entities
{
    public class ProductTag : BaseEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        // Navigation property
        public virtual Product.ProductEntity Product { get; set; }
    }
}
