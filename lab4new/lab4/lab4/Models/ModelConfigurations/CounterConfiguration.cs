using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lab4.Models.ModelConfigurations
{
    public class CounterConfiguration : IEntityTypeConfiguration<Counter>
    {
        public void Configure(EntityTypeBuilder<Counter> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Indications)
                   .WithOne(x => x.Counter)
                   .HasForeignKey(x => x.CounterId);

            builder.HasOne(x => x.PaymentType)
                   .WithMany(x => x.Counters)
                   .HasForeignKey(x => x.PaymentTypeId);
        }
    }
}
