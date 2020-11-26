using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .Property<string>("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(20);

            builder
                .Property<string>("_displayName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("DisplayName")
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property<string>("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Description")
                .IsRequired()
                .HasMaxLength(2000);

            var navigation = builder.Metadata.FindNavigation(nameof(Recipe.Steps));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
