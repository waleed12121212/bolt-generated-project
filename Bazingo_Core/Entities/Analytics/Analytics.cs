namespace Bazingo_Core.Entities
{
    public class SalesAnalytics : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public decimal CommissionEarned { get; set; }
        public int NewCustomers { get; set; }
        public int ReturnCustomers { get; set; }
        public decimal RefundAmount { get; set; }
        public int RefundCount { get; set; }
    }

    public class ProductAnalytics : BaseEntity
    {
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
        public int AddedToCart { get; set; }
        public int AddedToWishlist { get; set; }
        public int PurchaseCount { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }

        // Navigation property
        public virtual Product.ProductEntity Product { get; set; }
    }

    public class UserAnalytics : BaseEntity
    {
        public DateTime Date { get; set; }
        public int TotalUsers { get; set; }
        public int NewUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int BuyerCount { get; set; }
        public int SellerCount { get; set; }
        public decimal AverageSessionDuration { get; set; }
        public int SessionCount { get; set; }
    }

    public class SearchAnalytics : BaseEntity
    {
        public string SearchTerm { get; set; }
        public DateTime Date { get; set; }
        public int SearchCount { get; set; }
        public int ResultCount { get; set; }
        public int ClickCount { get; set; }
        public int ConversionCount { get; set; }
        public decimal CTR { get; set; }
        public decimal ConversionRate { get; set; }
    }
}
