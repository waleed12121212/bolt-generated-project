using Bazingo_Core.Entities.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItemEntity>
    {
        public void Configure(EntityTypeBuilder<WishlistItemEntity> builder)
        {
            builder.HasKey(wi => wi.Id);

            builder.Property(wi => wi.CreatedAt)
                .IsRequired();

            builder.Property(wi => wi.LastUpdated)
                .IsRequired();

            builder.Property(wi => wi.IsDeleted)
                .IsRequired();

            builder.Property(wi => wi.ProductId)
                .IsRequired();

            builder.Property(wi => wi.WishlistId)
                .IsRequired();

            builder.Property(wi => wi.PriceWhenAdded)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(wi => wi.Product)
                .WithMany()
                .HasForeignKey(wi => wi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wi => wi.Wishlist)
                .WithMany(w => w.Items)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
