using Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adams.Services.Smoking.Infrastructure.EntityConfigurations
{
    internal class RecipeEntityTypeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("recipes", SmokingContext.DEFAULT_SCHEMA);

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.DomainEvents);

            builder.Property(x => x.Id)
                .UseHiLo("recipeseq", SmokingContext.DEFAULT_SCHEMA);

            builder
                .Property(x => x.Name)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasMaxLength(20);

            builder
                .Property(x => x.DisplayName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(x => x.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasMaxLength(2000);

            var navigation = builder.Metadata.FindNavigation(nameof(Recipe.Steps));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}