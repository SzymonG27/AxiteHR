﻿// <auto-generated />
using System;
using AxiteHR.Services.ApplicationAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AxiteHR.Services.ApplicationAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240922180349_InitialApplicationMigration")]
    partial class InitialApplicationMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AxiteHR.Services.ApplicationAPI.Models.Application.UserApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationStatus")
                        .HasColumnType("int");

                    b.Property<int>("ApplicationType")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InsDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InsUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UpdUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("UserId"), false);

                    b.ToTable("UserApplications");
                });

            modelBuilder.Entity("AxiteHR.Services.ApplicationAPI.Models.Application.UserApplicationSupervisorAccepted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SupervisorAcceptedId")
                        .HasColumnType("int");

                    b.Property<int>("UserApplicationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupervisorAcceptedId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("SupervisorAcceptedId"), false);

                    b.HasIndex("UserApplicationId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("UserApplicationId"), false);

                    b.ToTable("UserApplicationSupervisorAccepteds");
                });

            modelBuilder.Entity("AxiteHR.Services.ApplicationAPI.Models.Application.UserCompanyDaysOff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationType")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<decimal?>("DaysOff")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InsDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("InsUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UpdUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("CompanyId"), false);

                    b.HasIndex("UserId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("UserId"), false);

                    b.HasIndex("UserId", "CompanyId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("UserId", "CompanyId"), false);

                    b.ToTable("UserCompanyDaysOffs");
                });

            modelBuilder.Entity("AxiteHR.Services.ApplicationAPI.Models.Application.UserApplicationSupervisorAccepted", b =>
                {
                    b.HasOne("AxiteHR.Services.ApplicationAPI.Models.Application.UserApplication", "UserApplication")
                        .WithMany()
                        .HasForeignKey("UserApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserApplication");
                });
#pragma warning restore 612, 618
        }
    }
}
