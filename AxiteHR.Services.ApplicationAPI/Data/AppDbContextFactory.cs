using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using AxiteHR.Services.ApplicationAPI.Helpers;

namespace AxiteHR.Services.ApplicationAPI.Data
{
	public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString(ConfigurationHelper.DefaultConnectionString));

			return new AppDbContext(optionsBuilder.Options);
		}
	}
}
