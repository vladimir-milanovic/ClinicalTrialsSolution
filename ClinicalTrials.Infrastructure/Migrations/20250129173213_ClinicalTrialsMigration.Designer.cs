﻿// <auto-generated />
using System;
using ClinicalTrials.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClinicalTrials.Infrastructure.Migrations
{
    [DbContext(typeof(ClinicalTrialsDbContext))]
    [Migration("20250129173213_ClinicalTrialsMigration")]
    partial class ClinicalTrialsMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClinicalTrials.Domain.Entities.ClinicalTrialMetadata", b =>
                {
                    b.Property<string>("TrialId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("Participants")
                        .HasColumnType("int");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrialId");

                    b.ToTable("ClinicalTrialMetadata");
                });
#pragma warning restore 612, 618
        }
    }
}
