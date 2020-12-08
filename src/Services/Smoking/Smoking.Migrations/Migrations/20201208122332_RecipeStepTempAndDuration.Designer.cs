﻿// <auto-generated />
using System;
using Adams.Services.Smoking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adams.Services.Smoking.Migrations
{
    [DbContext(typeof(SmokingContext))]
    [Migration("20201208122332_RecipeStepTempAndDuration")]
    partial class RecipeStepTempAndDuration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Protein", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("protein", "smoker");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "none"
                        },
                        new
                        {
                            Id = 2,
                            Name = "pork"
                        },
                        new
                        {
                            Id = 3,
                            Name = "beef"
                        },
                        new
                        {
                            Id = 4,
                            Name = "poultry"
                        },
                        new
                        {
                            Id = 5,
                            Name = "lamb"
                        });
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("recipeseq", "smoker");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ProteinId")
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("ProteinId");

                    b.ToTable("recipes", "smoker");
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.RecipeStep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("recipestepseq");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.Property<double?>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("recipeSteps", "smoker");
                });

            modelBuilder.Entity("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Recipe", b =>
                {
                    b.HasOne("Adams.Services.Smoking.Domain.AggregatesModel.RecipeAggregate.Protein", "Protein")
                        .WithMany()
                        .HasForeignKey("ProteinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Protein");
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
