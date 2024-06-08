using AxiteHr.Services.CompanyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AxiteHr.Services.CompanyAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Company> Companies { get; set; }
		public DbSet<CompanyLevel> CompanyLevels { get; set; }
		public DbSet<CompanyUser> CompanyUsers { get; set; }
		public DbSet<CompanyRole> CompanyRoles { get; set; }
		public DbSet<CompanyUserRole> CompanyUserRoles { get; set; }
		public DbSet<CompanyPermission> CompanyPermissions { get; set; }
		public DbSet<CompanyUserPermission> CompanyUserPermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			modelBuilder.Entity<CompanyPermission>()
				.Property(x => x.Id)
				.ValueGeneratedNever();
			modelBuilder.Entity<CompanyPermission>()
				.HasData(
					new CompanyPermission { Id = 1, PermissionName = "Creator" },
					new CompanyPermission { Id = 2, PermissionName = "Authorized to manage" },
					new CompanyPermission { Id = 3, PermissionName = "Employee" }
				);

			modelBuilder.Entity<CompanyRole>()
				.HasData(
					new CompanyRole { Id = 1, RoleName = "Software department", IsMain = true }
				);
		}
	}
}
