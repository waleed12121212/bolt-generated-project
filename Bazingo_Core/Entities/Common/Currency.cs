namespace Bazingo_Core.Entities
{
    public class Currency : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation properties
        public virtual ICollection<PriceHistory> PriceHistories { get; set; }
    }
}
