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
    public class ProteinEntityTypeConfiguration : IEntityTypeConfiguration<Protein>
    {
        public void Configure(EntityTypeBuilder<Protein> builder)
        {
            builder.ToTable("protein", SmokingContext.DEFAULT_SCHEMA);

            builder.Property(p => p.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(Protein.List());
        }
    }
}
