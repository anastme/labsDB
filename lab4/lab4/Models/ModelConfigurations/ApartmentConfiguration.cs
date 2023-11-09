using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace lab4.Models.ModelConfigurations
{
    public class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Square);
            builder.Property(x => x.FirstName);
            builder.Property(x => x.LastName);
            builder.Property(x => x.MiddleName);
            builder.Property(x => x.HumanCount);

            builder.HasMany(x => x.Indications)
                   .WithOne(x => x.Apartment)
                   .HasForeignKey(x => x.ApartmentId);
        }
    }
}
