using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.ApplicationAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		protected override void OnModelCreating(ModelBuilder modelBuilder) { }
	}
}
