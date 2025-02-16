using Bazingo_Core.Entities.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class AuctionConfiguration : IEntityTypeConfiguration<AuctionEntity>
    {
        public void Configure(EntityTypeBuilder<AuctionEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.StartingPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.MinimumBidIncrement)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.StartTime)
                .IsRequired();

            builder.Property(a => a.EndTime)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            // Product relationship
            builder.HasOne(a => a.Product)
                .WithOne()
                .HasForeignKey<AuctionEntity>(a => a.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Seller relationship
            builder.HasOne(a => a.Seller)
                .WithMany()
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Winner relationship
            builder.HasOne(a => a.Winner)
                .WithMany()
                .HasForeignKey(a => a.WinnerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Bids relationship
            builder.HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
