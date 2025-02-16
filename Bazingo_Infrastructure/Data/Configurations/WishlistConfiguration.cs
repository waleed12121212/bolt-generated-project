using Bazingo_Core.Entities.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<WishlistEntity>
    {
        public void Configure(EntityTypeBuilder<WishlistEntity> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId)
                .IsRequired();

            builder.Property(w => w.LastUpdated)
                .IsRequired();

            // User relationship
            builder.HasOne(w => w.User)
                .WithOne()
                .HasForeignKey<WishlistEntity>(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Items relationship
            builder.HasMany(w => w.Items)
                .WithOne(i => i.Wishlist)
                .HasForeignKey(i => i.WishlistId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(w => w.UserId);
        }
    }
}
