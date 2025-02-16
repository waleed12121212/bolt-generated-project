using Bazingo_Core.Entities.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<BidEntity>
    {
        public void Configure(EntityTypeBuilder<BidEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.BidTime)
                .IsRequired();

            builder.Property(b => b.BidderId)
                .IsRequired();

            builder.HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Bidder)
                .WithMany()
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
