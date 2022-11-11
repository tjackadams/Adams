using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.WeightTracker.Api.Domain;

namespace Nexus.WeightTracker.Api.Infrastructure;

public class WeightDbContext : DbContext
{
    public WeightDbContext(DbContextOptions<WeightDbContext> options)
        : base(options)
    {

    }

    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Weight");

        modelBuilder.Entity<Client>(e =>
        {
            e.HasKey(p => p.ClientId);

            e.Property(p => p.ClientId)
                .HasConversion(new ValueConverter<ClientId, int>(c => c.Value, c => new ClientId(c)))
                .ValueGeneratedOnAdd();

            e.Property(p => p.CreatedTime)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            e.Property(p => p.Name)
                .HasMaxLength(Client.MaximumNameLength);

            e.Property(p => p.Version)
                .IsRowVersion();

            var metrics = e.Metadata.FindNavigation(nameof(Client.Metrics));
            metrics?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<ClientMetric>(e =>
        {
            e.HasKey(p => p.ClientMetricId);

            e.Property(p => p.ClientMetricId)
                .HasConversion(new ValueConverter<ClientMetricId, int>(c => c.Value, c => new ClientMetricId(c)))
                .ValueGeneratedOnAdd();

            e.Property(p => p.RecordedValue)
                .HasPrecision(18, 2);

            e.Property(p => p.RecordedDate)
                .HasConversion(new ValueConverter<DateOnly, DateTime>(d => d.ToDateTime(TimeOnly.MinValue), d => DateOnly.FromDateTime(d)))
                .HasColumnType("date");

            e.Property(p => p.CreatedTime)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        });
    }
}
