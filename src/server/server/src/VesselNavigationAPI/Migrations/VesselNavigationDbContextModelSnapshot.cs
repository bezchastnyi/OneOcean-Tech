﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VesselNavigationAPI.DB;

namespace VesselNavigationAPI.Migrations
{
    [DbContext(typeof(VesselNavigationDbContext))]
    partial class VesselNavigationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("VesselNavigationAPI.Models.Db.Vessel", b =>
                {
                    b.Property<Guid>("VesselId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("VesselId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Vessel");
                });

            modelBuilder.Entity("VesselNavigationAPI.Models.Db.VesselPosition", b =>
                {
                    b.Property<Guid>("VesselPositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("VesselId")
                        .HasColumnType("uuid");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("VesselPositionId");

                    b.HasIndex("VesselId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("VesselNavigationAPI.Models.Db.VesselPosition", b =>
                {
                    b.HasOne("VesselNavigationAPI.Models.Db.Vessel", null)
                        .WithMany()
                        .HasForeignKey("VesselId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}