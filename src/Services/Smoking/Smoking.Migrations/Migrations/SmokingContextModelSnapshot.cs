﻿// <auto-generated />
using Adams.Services.Smoking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adams.Services.Smoking.Migrations
{
    [DbContext(typeof(SmokingContext))]
    partial class SmokingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.HasSequence("recipeseq", "smoker")
                .IncrementsBy(10);

            modelBuilder.HasSequence("recipestepseq")
                .IncrementsBy(10);

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("recipeseq", "smoker");

                    b.Property<string>("_description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("Description");

                    b.Property<string>("_displayName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("DisplayName");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("recipes", "smoker");
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.RecipeStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("recipestepseq");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("_description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasColumnName("Description");

                    b.Property<int>("_step")
                        .HasColumnType("int")
                        .HasColumnName("Step");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("recipeSteps", "smoker");
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.RecipeStep", b =>
                {
                    b.HasOne("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Recipe", null)
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Recipe", b =>
                {
                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}