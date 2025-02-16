using System;

namespace Bazingo_Core.Entities
{
    public class PriceHistory : BaseEntity
    {
        public int ProductId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Price { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Reason { get; set; }
        public string UpdatedBy { get; set; }

        // Navigation properties
        public virtual Product.ProductEntity Product { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
