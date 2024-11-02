using AxiteHR.Services.ApplicationAPI.Models.Application;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.ApplicationAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<UserCompanyDaysOff> UserCompanyDaysOffs { get; set; }

		public DbSet<UserApplication> UserApplications { get; set; }

		public DbSet<UserApplicationSupervisorAccepted> UserApplicationSupervisorAccepteds { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserCompanyDaysOff>()
				.HasIndex(uc => uc.CompanyUserId)
				.IsClustered(false);

			modelBuilder.Entity<UserApplication>()
				.HasIndex(ua => ua.CompanyUserId)
				.IsClustered(false);

			modelBuilder.Entity<UserApplicationSupervisorAccepted>()
				.HasIndex(uasa => uasa.UserApplicationId)
				.IsClustered(false);

			modelBuilder.Entity<UserApplicationSupervisorAccepted>()
				.HasIndex(uasa => uasa.SupervisorAcceptedId)
				.IsClustered(false);
		}
	}
}
