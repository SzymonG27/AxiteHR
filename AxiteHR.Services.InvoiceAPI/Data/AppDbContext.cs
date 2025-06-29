using AxiteHR.Integration.GlobalClass.Enums.Invoice;
using AxiteHR.Services.InvoiceAPI.Models;
using AxiteHR.Services.InvoiceAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.InvoiceAPI.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Invoice> Invoices { get; set; }

		public DbSet<InvoicePosition> InvoicePositions { get; set; }

		public DbSet<InvoiceSequence> InvoiceSequences { get; set; }

		// Model configuration
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Invoice>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_Status_Enum", EnumCheckConstraint<InvoiceStatus>(nameof(Invoice.Status)))
				);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_Type_Enum", EnumCheckConstraint<InvoiceType>(nameof(Invoice.Type)))
				);

				entity.Property(e => e.Number)
					.IsRequired()
					.HasMaxLength(50)
					.IsUnicode(false);

				entity.Property(e => e.BlobFileName)
					.IsRequired()
					.HasMaxLength(100)
					.IsUnicode(false);

				entity.HasIndex(e => e.BlobFileName)
					.IsUnique();

				entity.Property(e => e.ClientName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.ClientNip)
					.IsRequired()
					.HasMaxLength(10)
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_ClientNip_NIP_Format", "LEN([ClientNip]) = 10 AND [ClientNip] NOT LIKE '%[^0-9]%'")
				);

				entity.Property(e => e.ClientStreet)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.ClientHouseNumber)
					.IsRequired()
					.HasMaxLength(30)
					.IsUnicode(false);

				entity.Property(e => e.ClientPostalCode)
					.IsRequired()
					.HasMaxLength(6)
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_ClientPostalCode_PostalCode_Format", "[ClientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'")
				);

				entity.Property(e => e.ClientCity)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.RecipientName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.RecipientNip)
					.IsRequired()
					.HasMaxLength(10)
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_RecipientNip_NIP_Format", "LEN([RecipientNip]) = 10 AND [RecipientNip] NOT LIKE '%[^0-9]%'")
				);

				entity.Property(e => e.RecipientStreet)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.RecipientHouseNumber)
					.IsRequired()
					.HasMaxLength(30)
					.IsUnicode(false);

				entity.Property(e => e.RecipientPostalCode)
					.IsRequired()
					.HasMaxLength(6)
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_RecipientPostalCode_PostalCode_Format", "[RecipientPostalCode] LIKE '[0-9][0-9]-[0-9][0-9][0-9]'")
				);

				entity.Property(e => e.RecipientCity)
					.IsRequired()
					.HasMaxLength(100);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_PaymentMethod_Enum", EnumCheckConstraint<PaymentMethod>(nameof(Invoice.PaymentMethod)))
				);

				entity.Property(e => e.BankAccountNumber)
					.HasMaxLength(26)
					.IsUnicode(false);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_BankAccountNumber_Format", "[BankAccountNumber] IS NULL OR (LEN([BankAccountNumber]) = 26 AND [BankAccountNumber] NOT LIKE '%[^0-9]%')")
				);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_BankAccountNumber_RequiredIfPaymentByTransfer", $"([PaymentMethod] <> {(int)PaymentMethod.Transfer}) OR ([BankAccountNumber] IS NOT NULL AND LTRIM(RTRIM([BankAccountNumber])) <> '')")
				);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_Currency_Enum", EnumCheckConstraint<Currency>(nameof(Invoice.Currency)))
				);

				entity.Property(e => e.NetAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.Property(e => e.GrossAmount)
					.IsRequired()
					.HasColumnType("decimal(18,2)");

				entity.HasMany(i => i.InvoicePositions)
					.WithOne(p => p.Invoice)
					.HasForeignKey(p => p.InvoiceId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<InvoicePosition>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.ProductName)
					.IsRequired()
					.HasMaxLength(100);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_Unit_Enum", EnumCheckConstraint<Unit>(nameof(InvoicePosition.Unit)))
				);

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

			modelBuilder.Entity<InvoiceSequence>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.ToTable(t =>
					t.HasCheckConstraint("CK_Type_Enum", EnumCheckConstraint<InvoiceType>(nameof(InvoiceSequence.Type)))
				);

				entity.HasIndex(e => new { e.CompanyUserId, e.Type, e.Year, e.Month })
					.IsUnique();

				entity.Property(e => e.CurrentNumber)
					.IsRequired();
			});
		}

		private static string EnumCheckConstraint<TEnum>(string columnName) where TEnum : Enum
		{
			var values = string.Join(", ", Enum.GetValues(typeof(TEnum)).Cast<int>());
			return $"[{columnName}] IN ({values})";
		}
	}
}
