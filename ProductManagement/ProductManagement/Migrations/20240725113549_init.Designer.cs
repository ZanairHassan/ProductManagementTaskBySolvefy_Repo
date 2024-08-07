﻿// <auto-generated />
using System;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ProductManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240725113549_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsModified")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ProductCreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProductDeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ProductModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("ProductPrice")
                        .HasPrecision(16, 2)
                        .HasColumnType("decimal(16,2)");

                    b.Property<string>("ProductType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DAL.Models.ProductType", b =>
                {
                    b.Property<int>("ProductTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductTypeId"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ProductTypeCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProductTypeDeleted")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductTypeDes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("ProductTypeUpdated")
                        .HasColumnType("datetime2");

                    b.HasKey("ProductTypeId");

                    b.ToTable("ProductTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
