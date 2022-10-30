using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.Todo.Api.Domain;

namespace Nexus.Todo.Api.Infrastructure;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {

    }

    public DbSet<Domain.Todo> Todos => Set<Domain.Todo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Todo");

        modelBuilder.Entity<Domain.Todo>(e =>
        {
            e.HasKey(p => p.TodoId);

            e.Property(p => p.TodoId)
                .HasConversion(new ValueConverter<TodoId, int>(c => c.Value, c => new TodoId(c)))
                .ValueGeneratedOnAdd();

            e.Property(p => p.CreatedTime)
                .ValueGeneratedOnAdd();

            e.Property(p => p.Title)
                .HasMaxLength(Domain.Todo.MaximumTitleLength);

            var navigation = e.Metadata.FindNavigation(nameof(Domain.Todo.Tasks));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<Domain.TodoTask>(e =>
        {
            e.HasKey(p => p.TodoTaskId);

            e.Property(p => p.TodoTaskId)
                .HasConversion(new ValueConverter<TodoTaskId, int>(c => c.Value, c => new TodoTaskId(c)))
                .ValueGeneratedOnAdd();
        });
    }
}
