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

			modelBuilder.Entity<UserCompanyDaysOff>(entity =>
			{
				entity.HasKey(uc => uc.Id);

				entity.HasIndex(uc => uc.CompanyUserId)
					.IsClustered(false);
			});

			modelBuilder.Entity<UserApplication>(entity =>
			{
				entity.HasKey(ua => ua.Id);

				entity.HasIndex(ua => ua.CompanyUserId)
					.IsClustered(false);

				entity.Property(ua => ua.Reason)
					.HasMaxLength(250);
			});

			modelBuilder.Entity<UserApplicationSupervisorAccepted>(entity =>
			{
				entity.HasKey(uasa => uasa.Id);

				entity.HasIndex(uasa => uasa.UserApplicationId)
					.IsClustered(false);

				entity.HasIndex(uasa => uasa.SupervisorAcceptedId)
					.IsClustered(false);

				entity.HasOne(uasa => uasa.UserApplication)
					.WithMany()
					.HasForeignKey(uasa => uasa.UserApplicationId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
	}
}
