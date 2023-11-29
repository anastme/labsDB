using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lab5.EF.ModelConfigurations
{
    public class IndicationConfiguration : IEntityTypeConfiguration<Indication>
    {
        public void Configure(EntityTypeBuilder<Indication> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Volume);
            builder.Property(x => x.Date);

            builder.HasOne(x => x.Counter)
                   .WithMany(x => x.Indications)
                   .HasForeignKey(x => x.CounterId);

            builder.HasOne(x => x.Apartment)
                   .WithMany(x => x.Indications)
                   .HasForeignKey(x => x.ApartmentId);
        }
    }
}
