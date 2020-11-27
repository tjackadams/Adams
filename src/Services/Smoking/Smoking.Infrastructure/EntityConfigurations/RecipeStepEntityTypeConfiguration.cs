using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Smoking.Infrastructure.EntityConfigurations
{
    public class RecipeStepEntityTypeConfiguration : IEntityTypeConfiguration<RecipeStep>
    {
        public void Configure(EntityTypeBuilder<RecipeStep> builder)
        {
            builder.ToTable("recipeSteps", SmokingContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.DomainEvents);

            builder.Property(x => x.Id).UseHiLo("recipestepseq");

            builder.Property(p => p.RecipeId).IsRequired();

            builder
                .Property(x => x.Step)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasMaxLength(2000);
        }
    }
}