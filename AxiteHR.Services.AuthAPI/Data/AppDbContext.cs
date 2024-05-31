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
				new IdentityRole { Name = "Admin", NormalizedName = "ADMIN"},
				new IdentityRole { Name = "User", NormalizedName = "USER" }
			);
		}
	}
}
