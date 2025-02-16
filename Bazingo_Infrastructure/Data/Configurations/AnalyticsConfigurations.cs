using Bazingo_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class SalesAnalyticsConfiguration : IEntityTypeConfiguration<SalesAnalytics>
    {
        public void Configure(EntityTypeBuilder<SalesAnalytics> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalSales)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.AverageOrderValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.CommissionEarned)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.RefundAmount)
                .HasColumnType("decimal(18,2)");
        }
    }

    public class ProductAnalyticsConfiguration : IEntityTypeConfiguration<ProductAnalytics>
    {
        public void Configure(EntityTypeBuilder<ProductAnalytics> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Revenue)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.AverageRating)
                .HasColumnType("decimal(3,2)");

            // Product relationship
            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
