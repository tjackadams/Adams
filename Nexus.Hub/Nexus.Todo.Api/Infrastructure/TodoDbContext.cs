using Microsoft.EntityFrameworkCore;

namespace Nexus.Todo.Api.Infrastructure
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {

        }

        public DbSet<Domain.Todo> Todos => Set<Domain.Todo>();
    }
}
