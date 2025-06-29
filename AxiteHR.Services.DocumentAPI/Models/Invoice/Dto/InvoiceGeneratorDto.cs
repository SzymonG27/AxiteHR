using AxiteHR.Integration.GlobalClass.Enums;
using AxiteHR.Integration.GlobalClass.Enums.Invoice;

namespace AxiteHR.Services.DocumentAPI.Models.Invoice.Dto
{
	public record InvoiceGeneratorDto
	{
		public string BlobFileName { get; set; } = string.Empty;

		public LogoDto Logo { get; set; } = new();

		public string ClientName { get; set; } = string.Empty;

		public string ClientNip { get; set; } = string.Empty;

		public string ClientStreet { get; set; } = string.Empty;

		public string ClientHouseNumber { get; set; } = string.Empty;

		public string ClientPostalCode { get; set; } = string.Empty;

		public string ClientCity { get; set; } = string.Empty;

		public string ClientAddress => GetClientAddress();

		public string RecipientName { get; set; } = string.Empty;

		public string RecipientNip { get; set; } = string.Empty;

		public string RecipientStreet { get; set; } = string.Empty;

		public string RecipientHouseNumber { get; set; } = string.Empty;

		public string RecipientPostalCode { get; set; } = string.Empty;

		public string RecipientCity { get; set; } = string.Empty;

		public string RecipientAddress => GetRecipientAddress();

		public DateTime IssueDate { get; set; }

		public DateTime SaleDate { get; set; }

		public PaymentMethod PaymentMethod { get; set; }

		public string PaymentMethodString => GetPaymentMethodString();

		public string BankAccountNumber { get; set; } = string.Empty;

		public DateTime PaymentDeadline { get; set; }

		public Currency Currency { get; set; }

		public bool IsSplitPayment { get; set; }

		public decimal NetAmount { get; set; }

		public decimal GrossAmount { get; set; }

		public decimal VatAmount { get; set; }

		public string InvoiceNumber { get; set; } = string.Empty;

		public Language Language { get; set; } = Language.pl;

		public Dictionary<string, string> Translations { get; set; } = [];

		public IList<InvoicePositionGeneratorDto> InvoicePositions { get; set; } = [];

		private string GetClientAddress()
		{
			return Language switch
			{
				Language.en => $"St. {ClientStreet} {ClientHouseNumber} {ClientPostalCode} {ClientCity}",
				_ => $"ul. {ClientStreet} {ClientHouseNumber} {ClientPostalCode} {ClientCity}",
			};
		}

		private string GetRecipientAddress()
		{
			return Language switch
			{
				Language.en => $"St. {RecipientStreet} {RecipientHouseNumber} {RecipientPostalCode} {RecipientCity}",
				_ => $"ul. {RecipientStreet} {RecipientHouseNumber} {RecipientPostalCode} {RecipientCity}",
			};
		}

		private string GetPaymentMethodString()
		{
			return PaymentMethod switch
			{
				PaymentMethod.Transfer => "Przelew",
				PaymentMethod.Cash => "Gotówka",
				PaymentMethod.Card => "Karta",
				_ => string.Empty,
			};
		}
	}
}
