    using Microsoft.EntityFrameworkCore;
    using Bazingo_Core.Entities;
    using Bazingo_Core.Entities.Shopping;
    using Bazingo_Core.Entities.Auction;
    using Bazingo_Core.Entities.Product;
    using Bazingo_Core.Entities.Payment;

    namespace Bazingo_Core.Interfaces
    {
        public interface IApplicationDbContext
        {
            DbSet<ProductEntity> Products { get; set; }
            DbSet<CategoryEntity> Categories { get; set; }
            DbSet<CartEntity> Carts { get; set; }
            DbSet<CartItemEntity> CartItems { get; set; }
            DbSet<OrderEntity> Orders { get; set; }
            DbSet<OrderItemEntity> OrderItems { get; set; }
            DbSet<WishlistEntity> Wishlists { get; set; }
            DbSet<WishlistItemEntity> WishlistItems { get; set; }
            DbSet<AuctionEntity> Auctions { get; set; }
            DbSet<BidEntity> Bids { get; set; }
            DbSet<ProductReviewEntity> ProductReviews { get; set; }
            DbSet<OrderStatusHistoryEntity> OrderStatusHistories { get; set; }
            DbSet<Payment> Payments { get; set; }
            DbSet<PriceHistory> PriceHistories { get; set; }
            DbSet<Currency> Currencies { get; set; }
            DbSet<EscrowTransaction> EscrowTransactions { get; set; }
            DbSet<SalesAnalytics> SalesAnalytics { get; set; }
            DbSet<ProductAnalytics> ProductAnalytics { get; set; }

            Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        }
    }
