using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lab6.EF.ModelConfigurations
{
    public class PaymentTypeConfiguration : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name);

            builder.HasMany(x => x.Counters)
                   .WithOne(x => x.PaymentType)
                   .HasForeignKey(x => x.PaymentTypeId);

            builder.HasMany(x => x.Rates)
                   .WithOne(x => x.PaymentType)
                   .HasForeignKey(x => x.PaymentTypeId);
        }
    }
}
