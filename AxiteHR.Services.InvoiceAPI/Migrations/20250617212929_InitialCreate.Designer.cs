﻿// <auto-generated />
using System;
using AxiteHR.Services.InvoiceAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AxiteHR.Services.InvoiceAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250617212929_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AxiteHR.Services.InvoiceAPI.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BankAccountNumber")
                        .HasMaxLength(26)
                        .IsUnicode(false)
                        .HasColumnType("varchar(26)");

                    b.Property<string>("BlobFileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ClientCity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ClientHouseNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ClientNip")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("ClientPostalCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("varchar(6)");

                    b.Property<string>("ClientStreet")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InsDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InsUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsSplitPayment")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("NetAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaymentDeadline")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UpdUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Invoices", t =>
                        {
                            t.HasCheckConstraint("CK_BankAccountNumber_Format", "[BankAccountNumber] IS NULL OR (LEN([BankAccountNumber]) = 26 AND [BankAccountNumber] NOT LIKE '%[^0-9]%')");

                            t.HasCheckConstraint("CK_BankAccountNumber_RequiredIfPaymentByTransfer", "([PaymentMethod] <> 2) OR ([BankAccountNumber] IS NOT NULL AND LTRIM(RTRIM([BankAccountNumber])) <> '')");

                            t.HasCheckConstraint("CK_ClientNip_NIP_Format", "LEN([ClientNip]) = 10 AND [ClientNip] NOT LIKE '%[^0-9]%'");

                            t.HasCheckConstraint("CK_ClientPostalCode_PostalCode_Format", "[ClientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'");

                            t.HasCheckConstraint("CK_Currency_Enum", "[Currency] IN (1, 2)");

                            t.HasCheckConstraint("CK_PaymentMethod_Enum", "[PaymentMethod] IN (1, 2, 3)");

                            t.HasCheckConstraint("CK_Status_Enum", "[Status] IN (1, 2, 3)");
                        });
                });

            modelBuilder.Entity("AxiteHR.Services.InvoiceAPI.Models.InvoicePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<decimal>("NetAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("NetPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.Property<decimal>("VatAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VatRate")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoicePositions", t =>
                        {
                            t.HasCheckConstraint("CK_Unit_Enum", "[Unit] IN (1, 2)");

                            t.HasCheckConstraint("CK_VatRate_IntRange", "[VatRate] >= 0 AND [VatRate] <= 100");
                        });
                });

            modelBuilder.Entity("AxiteHR.Services.InvoiceAPI.Models.InvoicePosition", b =>
                {
                    b.HasOne("AxiteHR.Services.InvoiceAPI.Models.Invoice", "Invoice")
                        .WithMany()
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");
                });
#pragma warning restore 612, 618
        }
    }
}
