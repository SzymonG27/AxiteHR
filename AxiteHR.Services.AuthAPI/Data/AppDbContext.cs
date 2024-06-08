using AxiteHR.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.AuthAPI.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<AppUser> AppUserList { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<AppUser>(b =>
			{
				b.Property(u => u.Email).IsRequired();
			});

			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = "c6b3a381-6ffb-4b88-8aa8-877421f520c7", Name = "Admin", NormalizedName = "ADMIN"},
				new IdentityRole { Id = "775da6ba-b138-4551-aa18-fb161e8ffbc9", Name = "User", NormalizedName = "USER" },
				new IdentityRole { Id = "87ec7457-f89e-4c4b-940d-31ec35e51e3f", Name = "UserFromCompany", NormalizedName = "USERFROMCOMPANY" }
			);
		}
	}
}
