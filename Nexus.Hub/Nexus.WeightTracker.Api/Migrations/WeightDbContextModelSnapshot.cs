﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.WeightTracker.Api.Infrastructure;

#nullable disable

namespace Nexus.WeightTracker.Api.Migrations
{
    [DbContext(typeof(WeightDbContext))]
    partial class WeightDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Weight")
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Nexus.WeightTracker.Api.Domain.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientId"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("ClientId");

                    b.ToTable("Clients", "Weight");
                });

            modelBuilder.Entity("Nexus.WeightTracker.Api.Domain.ClientMetric", b =>
                {
                    b.Property<int>("ClientMetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientMetricId"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("SYSDATETIMEOFFSET()");

                    b.Property<DateTime>("RecordedDate")
                        .HasColumnType("date");

                    b.Property<decimal>("RecordedValue")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ClientMetricId");

                    b.HasIndex("ClientId");

                    b.ToTable("ClientMetrics", "Weight");
                });

            modelBuilder.Entity("Nexus.WeightTracker.Api.Domain.ClientMetric", b =>
                {
                    b.HasOne("Nexus.WeightTracker.Api.Domain.Client", null)
                        .WithMany("Metrics")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("Nexus.WeightTracker.Api.Domain.Client", b =>
                {
                    b.Navigation("Metrics");
                });
#pragma warning restore 612, 618
        }
    }
}
