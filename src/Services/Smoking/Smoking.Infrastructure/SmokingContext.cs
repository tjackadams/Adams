using System.Diagnostics;
using System.IO;
using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Adams.Services.Smoking.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Adams.Services.Smoking.Infrastructure
{
    public sealed class SmokingContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "smoker";

        public DbSet<Recipe> Recipes { get; set; }

        public SmokingContext(DbContextOptions<SmokingContext> options)
            : base(options)
        {
            Debug.WriteLine("SmokingContext::ctor ->" + GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipeStepEntityTypeConfiguration());
        }
    }
}