using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		// DbSety
		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyLevel> CompanyLevels { get; set; }
		public DbSet<CompanyUser> CompanyUsers { get; set; }
		public DbSet<CompanyRole> CompanyRoles { get; set; }
		public DbSet<CompanyRoleCompany> CompanyRoleCompanies { get; set; }
		public DbSet<CompanyUserRole> CompanyUserRoles { get; set; }
		public DbSet<CompanyPermission> CompanyPermissions { get; set; }
		public DbSet<CompanyUserPermission> CompanyUserPermissions { get; set; }

		// Konfiguracja modeli
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Konfiguracja dla CompanyPermission
			modelBuilder.Entity<CompanyPermission>()
				.Property(x => x.Id)
				.ValueGeneratedNever();

			modelBuilder.Entity<CompanyPermission>()
				.HasData(
					new CompanyPermission { Id = 1, PermissionName = "CompanyManager" },
					new CompanyPermission { Id = 2, PermissionName = "Employee" }
				);

			// Konfiguracja dla CompanyRole
			modelBuilder.Entity<CompanyRole>()
				.HasData(
					new CompanyRole { Id = 1, RoleName = "Company creator" },
					new CompanyRole { Id = 2, RoleName = "Software department" }
				);

			// Konfiguracja dla CompanyRoleCompany
			modelBuilder.Entity<CompanyRoleCompany>()
				.HasIndex(crc => new { crc.CompanyRoleId, crc.CompanyId })
				.IsUnique();

			// Konfiguracja dla CompanyLevel
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