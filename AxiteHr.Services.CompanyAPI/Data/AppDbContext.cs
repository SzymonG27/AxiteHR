﻿using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		/// <summary>
		/// Used for tests to not seed data
		/// </summary>
		public bool SkipSeedData { get; set; } = false;

		// DbSets
		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyLevel> CompanyLevels { get; set; }
		public DbSet<CompanyUser> CompanyUsers { get; set; }
		public DbSet<CompanyRole> CompanyRoles { get; set; }
		public DbSet<CompanyRoleCompany> CompanyRoleCompanies { get; set; }
		public DbSet<CompanyUserRole> CompanyUserRoles { get; set; }
		public DbSet<CompanyPermission> CompanyPermissions { get; set; }
		public DbSet<CompanyUserPermission> CompanyUserPermissions { get; set; }

		// Model configuration
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configuration for CompanyPermission
			modelBuilder.Entity<CompanyPermission>()
				.Property(x => x.Id)
				.ValueGeneratedNever();

			if (!SkipSeedData)
			{
				modelBuilder.Entity<CompanyUser>()
					.HasIndex(x => new { x.UserId, x.CompanyId })
					.IsUnique();

				modelBuilder.Entity<CompanyPermission>()
					.HasData(
						new CompanyPermission { Id = 1, PermissionName = "CompanyManager" },
						new CompanyPermission { Id = 2, PermissionName = "Employee" },
						new CompanyPermission { Id = 3, PermissionName = "CompanyRoleSeeEntireList" },
						new CompanyPermission { Id = 4, PermissionName = "CompanyUserSeeEntireList" },
						new CompanyPermission { Id = 5, PermissionName = "CompanyRoleCreator" }
					);
			}

			// Configuration for CompanyRole
			if (!SkipSeedData)
			{
				modelBuilder.Entity<CompanyRole>()
				.HasData(
					new CompanyRole { Id = 1, RoleName = "Twórca firmy", RoleNameEng = "Company creator" },
					new CompanyRole { Id = 2, RoleName = "Dział oprogramowania", RoleNameEng = "Software department" }
				);

				modelBuilder.Entity<CompanyRole>()
				.Property(x => x.RoleName)
				.HasMaxLength(100)
				.UseCollation("SQL_Latin1_General_CP1_CI_AS");

				modelBuilder.Entity<CompanyRole>()
					.Property(x => x.RoleNameEng)
					.HasMaxLength(100)
					.UseCollation("SQL_Latin1_General_CP1_CI_AS");
			}

			modelBuilder.Entity<CompanyRole>()
				.HasIndex(x => new { x.RoleName, x.RoleNameEng })
				.IsUnique();

			// Configuration for CompanyRoleCompany
			modelBuilder.Entity<CompanyRoleCompany>()
				.HasIndex(crc => new { crc.CompanyRoleId, crc.CompanyId })
				.IsUnique();

			// Configuration for CompanyLevel
			if (!SkipSeedData)
			{
				modelBuilder.Entity<CompanyLevel>()
					.HasData(
						new CompanyLevel { Id = 1, MaxNumberOfWorkers = 10 },
						new CompanyLevel { Id = 2, MaxNumberOfWorkers = 25 },
						new CompanyLevel { Id = 3, MaxNumberOfWorkers = 50 },
						new CompanyLevel { Id = 4, MaxNumberOfWorkers = 100 },
						new CompanyLevel { Id = 5, MaxNumberOfWorkers = int.MaxValue }
					);
			}
		}
	}
}