﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NS.Cart.API.Data;

namespace NS.Cart.API.Migrations
{
    [DbContext(typeof(CartContext))]
    [Migration("20220104030007_Voucher")]
    partial class Voucher
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NS.Cart.API.Model.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CartId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("CartItens");
                });

            modelBuilder.Entity("NS.Cart.API.Model.CustomerCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AppliedVoucher")
                        .HasColumnType("bit");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("IDX_Customer");

                    b.ToTable("CustomerCarts");
                });

            modelBuilder.Entity("NS.Cart.API.Model.CartItem", b =>
                {
                    b.HasOne("NS.Cart.API.Model.CustomerCart", "CustomerCart")
                        .WithMany("Itens")
                        .HasForeignKey("CartId")
                        .IsRequired();

                    b.Navigation("CustomerCart");
                });

            modelBuilder.Entity("NS.Cart.API.Model.CustomerCart", b =>
                {
                    b.OwnsOne("NS.Cart.API.Model.Voucher", "Voucher", b1 =>
                        {
                            b1.Property<Guid>("CustomerCartId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("varchar(50)")
                                .HasColumnName("VoucherCode");

                            b1.Property<int>("DiscountType")
                                .HasColumnType("int")
                                .HasColumnName("DiscountType");

                            b1.Property<decimal?>("DiscountValue")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("DiscountValue");

                            b1.Property<decimal?>("Percentage")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Percentage");

                            b1.HasKey("CustomerCartId");

                            b1.ToTable("CustomerCarts");

                            b1.WithOwner()
                                .HasForeignKey("CustomerCartId");
                        });

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("NS.Cart.API.Model.CustomerCart", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
