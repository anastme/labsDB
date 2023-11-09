using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lab4.Models.ModelConfigurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price);
            builder.Property(x => x.Date);

            builder.HasOne(x => x.PaymentType)
                   .WithMany(x => x.Rates)
                   .HasForeignKey(x => x.PaymentTypeId);
        }
    }
}
