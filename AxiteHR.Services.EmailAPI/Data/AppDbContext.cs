using AxiteHR.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AxiteHr.Services.EmailAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<EmailLogger> EmailLoggers { get; set; }
	}
}
