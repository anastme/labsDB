using lab6.EF.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace lab6.EF;

public partial class UtilitiesContext : DbContext
{

    public UtilitiesContext(DbContextOptions<UtilitiesContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Indication> Indications { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApartmentConfiguration());
        modelBuilder.ApplyConfiguration(new CounterConfiguration());
        modelBuilder.ApplyConfiguration(new IndicationConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RateConfiguration());
    }
}
