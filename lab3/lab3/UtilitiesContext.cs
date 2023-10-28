using lab3.Utilities.Domain;
using Microsoft.EntityFrameworkCore;

namespace lab3;

public partial class UtilitiesContext : DbContext
{
    public UtilitiesContext()
    {
    }

    public UtilitiesContext(DbContextOptions<UtilitiesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<Indication> Indications { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //   => optionsBuilder.UseSqlServer(DbConnection.Instance.GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Apartmen__3214EC07AD2CCBF5");

            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.HumanCount).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.MiddleName).HasMaxLength(20);
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Counters__3214EC07A7ED3377");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Counters)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("FK_Counters_To_Types");
        });

        modelBuilder.Entity<Indication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Indicati__3214EC07F8801029");

            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.Apartment).WithMany(p => p.Indications)
                .HasForeignKey(d => d.ApartmentId)
                .HasConstraintName("FK_Indications_To_Apartments");

            entity.HasOne(d => d.Counter).WithMany(p => p.Indications)
                .HasForeignKey(d => d.CounterId)
                .HasConstraintName("FK_Indications_To_Counters");
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentT__3214EC07AD8A58C3");

            entity.HasIndex(e => e.Name, "UQ__PaymentT__737584F6D3FC76E6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rates__3214EC0717F345EB");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Rates)
                .HasForeignKey(d => d.PaymentTypeId)
                .HasConstraintName("FK_Rates_To_PaymentTypes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
