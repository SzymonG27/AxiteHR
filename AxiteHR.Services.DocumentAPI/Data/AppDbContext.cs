using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.DocumentAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
	}
}
