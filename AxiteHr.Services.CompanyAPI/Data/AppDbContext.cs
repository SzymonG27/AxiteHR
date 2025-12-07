using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
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

			modelBuilder.Entity<Company>(entity =>
			{
				entity.HasKey(c => c.Id);

				entity.Property(c => c.CompanyName)
					.HasMaxLength(100)
					.IsRequired();

				entity.HasOne(c => c.CompanyLevel)
					.WithMany()
					.HasForeignKey(c => c.CompanyLevelId);
			});

			modelBuilder.Entity<CompanyLevel>(entity => entity.HasKey(cl => cl.Id));

			modelBuilder.Entity<CompanyPermission>(entity =>
			{
				entity.HasKey(cp => cp.Id);

				entity.Property(cp => cp.Id)
					.ValueGeneratedNever();

				entity.Property(cp => cp.PermissionName)
					.HasMaxLength(100)
					.IsRequired();
			});

			modelBuilder.Entity<CompanyRole>(entity =>
			{
				entity.HasKey(cr => cr.Id);

				entity.Property(cr => cr.RoleName)
					.HasMaxLength(100);

				entity.Property(cr => cr.RoleNameEng)
					.HasMaxLength(100);

				entity.HasIndex(cr => new { cr.RoleName, cr.RoleNameEng })
					.IsUnique();
			});

			modelBuilder.Entity<CompanyRoleCompany>(entity =>
			{
				entity.HasKey(crc => crc.Id);

				entity.HasOne(crc => crc.Company)
					.WithMany()
					.HasForeignKey(crc => crc.CompanyId);

				entity.HasOne(crc => crc.CompanyRole)
					.WithMany()
					.HasForeignKey(crc => crc.CompanyRoleId);

				entity.HasIndex(crc => new { crc.CompanyRoleId, crc.CompanyId })
					.IsUnique();
			});

			modelBuilder.Entity<CompanyUser>(entity =>
			{
				entity.HasKey(cu => cu.Id);

				entity.HasOne(cu => cu.Company)
					.WithMany()
					.HasForeignKey(cu => cu.CompanyId);
			});

			modelBuilder.Entity<CompanyUserPermission>(entity =>
			{
				entity.HasKey(cup => cup.Id);

				entity.HasOne(cup => cup.CompanyUser)
					.WithMany()
					.HasForeignKey(cup => cup.CompanyUserId);

				entity.HasOne(cup => cup.CompanyPermission)
					.WithMany()
					.HasForeignKey(cup => cup.CompanyPermissionId);
			});

			modelBuilder.Entity<CompanyUserRole>(entity =>
			{
				entity.HasKey(cup => cup.Id);

				entity.HasOne(cup => cup.CompanyUser)
					.WithMany()
					.HasForeignKey(cup => cup.CompanyUserId);

				entity.HasOne(cup => cup.CompanyRoleCompany)
					.WithMany()
					.HasForeignKey(cup => cup.CompanyRoleCompanyId);
			});

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

				modelBuilder.Entity<CompanyRole>(entity =>
				{
					entity.HasData(
						new CompanyRole { Id = 1, RoleName = "Twórca firmy", RoleNameEng = "Company creator" },
						new CompanyRole { Id = 2, RoleName = "Dział oprogramowania", RoleNameEng = "Software department" }
					);

					entity.Property(cr => cr.RoleName)
						.UseCollation("SQL_Latin1_General_CP1_CI_AS");
					
					entity.Property(cr => cr.RoleNameEng)
						.UseCollation("SQL_Latin1_General_CP1_CI_AS");
				});

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