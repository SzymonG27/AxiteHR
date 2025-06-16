using AxiteHR.Services.InvoiceAPI.Models;
using AxiteHR.Services.InvoiceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.InvoiceAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Invoice> Invoices { get; set; }

		public DbSet<InvoicePosition> InvoicePositions { get; set; }

		// Model configuration
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Invoice>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.BlobFileName)
					.IsRequired()
					.HasMaxLength(100)
					.IsFixedLength()
					.IsUnicode(false);

				entity.Property(e => e.ClientName)
					.IsRequired()
					.HasMaxLength(100)
					.IsFixedLength()
					.IsUnicode(false);

				entity.Property(e => e.ClientNip)
					.IsRequired()
					.HasMaxLength(10)
					.IsFixedLength()
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_ClientNip_NIP_Format", "LEN([ClientNip]) = 10 AND [ClientNip] NOT LIKE '%[^0-9]%'")
				);

				entity.Property(e => e.ClientStreet)
					.IsRequired()
					.HasMaxLength(100)
					.IsFixedLength()
					.IsUnicode(false);

				entity.Property(e => e.ClientHouseNumber)
					.IsRequired()
					.HasMaxLength(30)
					.IsFixedLength()
					.IsUnicode(false);

				entity.Property(e => e.ClientPostalCode)
					.IsRequired()
					.HasMaxLength(6)
					.IsFixedLength()
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_ClientPostalCode_PostalCode_Format", "[ClientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'")
				);

				entity.Property(e => e.ClientCity)
					.IsRequired()
					.HasMaxLength(100)
					.IsFixedLength()
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_BankAccountNumber_RequiredIfPaymentByTransfer", $"([PaymentMethod] <> {PaymentMethod.Transfer}) OR ([BankAccountNumber] IS NOT NULL AND LTRIM(RTRIM([BankAccountNumber])) <> '')")
				);

				entity.Property(e => e.NetAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.GrossAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");
			});

			modelBuilder.Entity<InvoicePosition>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.Invoice)
					.WithMany()
					.HasForeignKey(e => e.InvoiceId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.Property(e => e.ProductName)
					.IsRequired()
					.HasMaxLength(100)
					.IsFixedLength()
					.IsUnicode(false);

				entity.Property(e => e.Quantity)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.NetPrice)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.VatRate)
					.IsRequired();

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_VatRate_IntRange", "[VatRate] >= 0 AND [VatRate] <= 100")
				);

				entity.Property(e => e.VatAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.NetAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.GrossAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");
			});
		}
	}
}
