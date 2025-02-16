namespace Bazingo_Core.Entities
{
    public class ProductImage : BaseEntity
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public string AltText { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation property
        public virtual Product.ProductEntity Product { get; set; }
    }
}
