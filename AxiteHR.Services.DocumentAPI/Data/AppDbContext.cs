using AxiteHR.Services.DocumentAPI.Models.Invoice;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.DocumentAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<InvoiceGeneratorLog> InvoiceGeneratorLogs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<InvoiceGeneratorLog>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.InvoiceId)
					.IsRequired();

				entity.Property(e => e.FileExtension)
					.IsRequired();

				entity.Property(e => e.GeneratedDate)
					.IsRequired();
			});
		}
	}
}
