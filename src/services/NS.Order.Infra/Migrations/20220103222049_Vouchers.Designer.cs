﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NS.Order.Infra.Data;

namespace NS.Order.Infra.Migrations
{
    [DbContext(typeof(OrderContext))]
    [Migration("20220103222049_Vouchers")]
    partial class Vouchers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NS.Order.Domain.Vouchers.Voucher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiscountType")
                        .HasColumnType("int");

                    b.Property<decimal?>("DiscountValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UsageDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Used")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Vouchers");
                });
#pragma warning restore 612, 618
        }
    }
}
