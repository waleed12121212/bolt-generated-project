using Bazingo_Core.Entities.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bazingo_Core.Enums;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistoryEntity> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(h => h.ChangedDate)
                .IsRequired();

            builder.Property(h => h.Comment)
                .HasMaxLength(500);

            builder.HasOne(h => h.Order)
                .WithMany(o => o.StatusHistory)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
