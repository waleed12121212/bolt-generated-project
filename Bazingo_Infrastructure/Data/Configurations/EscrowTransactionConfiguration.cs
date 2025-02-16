using Bazingo_Core.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bazingo_Infrastructure.Data.Configurations
{
    public class EscrowTransactionConfiguration : IEntityTypeConfiguration<EscrowTransaction>
    {
        public void Configure(EntityTypeBuilder<EscrowTransaction> builder)
        {
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
