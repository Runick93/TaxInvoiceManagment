﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaxInvoiceManagment.Persistence.DbContexts;

#nullable disable

namespace TaxInvoiceManagment.Persistence.Migrations
{
    [DbContext(typeof(TaxInvoiceManagmentDbContext))]
    [Migration("20250222232246_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InvoiceAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("InvoiceReceiptPath")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentReceiptPath")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool?>("PaymentStatus")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PrimaryDueDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SecondaryDueDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TaxId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TaxId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.Tax", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AnnualPayment")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("AutoDebit")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClientNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Owner")
                        .HasColumnType("TEXT");

                    b.Property<int>("PayFrequency")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ServiceDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceType")
                        .HasColumnType("TEXT");

                    b.Property<int>("TaxableItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TaxableItemId");

                    b.ToTable("TaxesOrServices");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.TaxableItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VehicleNumberPlate")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TaxableItems");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.Invoice", b =>
                {
                    b.HasOne("TaxInvoiceManagment.Domain.Entities.Tax", "Tax")
                        .WithMany("Invoices")
                        .HasForeignKey("TaxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tax");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.Tax", b =>
                {
                    b.HasOne("TaxInvoiceManagment.Domain.Entities.TaxableItem", "TaxableItem")
                        .WithMany("Taxes")
                        .HasForeignKey("TaxableItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaxableItem");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.TaxableItem", b =>
                {
                    b.HasOne("TaxInvoiceManagment.Domain.Entities.User", "User")
                        .WithMany("TaxableItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.Tax", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.TaxableItem", b =>
                {
                    b.Navigation("Taxes");
                });

            modelBuilder.Entity("TaxInvoiceManagment.Domain.Entities.User", b =>
                {
                    b.Navigation("TaxableItems");
                });
#pragma warning restore 612, 618
        }
    }
}
