    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Bazingo_Core.Entities.Identity;
    using Bazingo_Core.Entities;
    using Bazingo_Core.Entities.Shopping;
    using Bazingo_Core.Entities.Auction;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Product;
    using Bazingo_Core.Entities.Payment;
    using Bazingo_Infrastructure.Data.Configurations;
    using Bazingo_Core.Interfaces;

    namespace Bazingo_Infrastructure.Data
    {
        public class ApplicationDbContext : DbContext, IApplicationDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {
            }

            public DbSet<ProductEntity> Products { get; set; }
            public DbSet<CategoryEntity> Categories { get; set; }
            public DbSet<CartEntity> Carts { get; set; }
            public DbSet<CartItemEntity> CartItems { get; set; }
            public DbSet<OrderEntity> Orders { get; set; }
            public DbSet<OrderItemEntity> OrderItems { get; set; }
            public DbSet<WishlistEntity> Wishlists { get; set; }
            public DbSet<WishlistItemEntity> WishlistItems { get; set; }
            public DbSet<AuctionEntity> Auctions { get; set; }
            public DbSet<BidEntity> Bids { get; set; }
            public DbSet<ProductReviewEntity> ProductReviews { get; set; }
            public DbSet<OrderStatusHistoryEntity> OrderStatusHistories { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<PriceHistory> PriceHistories { get; set; }
            public DbSet<Currency> Currencies { get; set; }
            public DbSet<EscrowTransaction> EscrowTransactions { get; set; }
            public DbSet<SalesAnalytics> SalesAnalytics { get; set; }
            public DbSet<ProductAnalytics> ProductAnalytics { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // Configure ApplicationUser as external table
                modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", t => t.ExcludeFromMigrations());

                // Apply all configurations
                modelBuilder.ApplyConfiguration(new ProductConfiguration());
                modelBuilder.ApplyConfiguration(new CategoryConfiguration());
                modelBuilder.ApplyConfiguration(new CartConfiguration());
                modelBuilder.ApplyConfiguration(new CartItemEntityConfiguration());
                modelBuilder.ApplyConfiguration(new OrderConfiguration());
                modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
                modelBuilder.ApplyConfiguration(new WishlistConfiguration());
                modelBuilder.ApplyConfiguration(new WishlistItemConfiguration());
                modelBuilder.ApplyConfiguration(new AuctionConfiguration());
                modelBuilder.ApplyConfiguration(new BidConfiguration());
                modelBuilder.ApplyConfiguration(new ProductReviewConfiguration());
                modelBuilder.ApplyConfiguration(new OrderStatusHistoryConfiguration());
                modelBuilder.ApplyConfiguration(new PaymentConfiguration());
                modelBuilder.ApplyConfiguration(new PriceHistoryConfiguration());
                modelBuilder.ApplyConfiguration(new EscrowTransactionConfiguration());
                modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
                modelBuilder.ApplyConfiguration(new SalesAnalyticsConfiguration());
                modelBuilder.ApplyConfiguration(new ProductAnalyticsConfiguration());
            }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                foreach (var entry in ChangeTracker.Entries<BaseEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedAt = DateTime.UtcNow;
                            entry.Entity.LastUpdated = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastUpdated = DateTime.UtcNow;
                            break;
                    }
                }

                return base.SaveChangesAsync(cancellationToken);
            }
        }
    }
