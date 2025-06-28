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

		public DateTime IssueDate { get; set; }

		public DateTime SaleDate { get; set; }

		public PaymentMethod PaymentMethod { get; set; }

		public string BankAccountNumber { get; set; } = string.Empty;

		public DateTime PaymentDeadline { get; set; }

		public Currency Currency { get; set; }

		public bool IsSplitPayment { get; set; }

		public decimal NetAmount { get; set; }

		public decimal GrossAmount { get; set; }

		public string InvoiceNumber { get; set; } = string.Empty;

		public IList<InvoicePositionGeneratorDto> InvoicePositions { get; set; } = [];
	}
}
