using AxiteHR.Integration.GlobalClass.Enums.Invoice;
using AxiteHR.Services.InvoiceAPI.Models.Enums;

namespace AxiteHR.Services.InvoiceAPI.Models
{
	public class Invoice
	{
		public virtual int Id { get; set; }

		public virtual InvoiceStatus Status { get; set; }

		public virtual InvoiceType Type { get; set; }

		public virtual string Number { get; set; } = string.Empty;

		public virtual int CompanyId { get; set; }

		public virtual int CompanyUserId { get; set; }

		public virtual string BlobFileName { get; set; } = string.Empty;

		public virtual string ClientName { get; set; } = string.Empty;

		public virtual string ClientNip { get; set; } = string.Empty;

		public virtual string ClientStreet { get; set; } = string.Empty;

		public virtual string ClientHouseNumber { get; set; } = string.Empty;

		public virtual string ClientPostalCode { get; set; } = string.Empty;

		public virtual string ClientCity { get; set; } = string.Empty;

		public virtual string RecipientName { get; set; } = string.Empty;

		public virtual string RecipientNip { get; set; } = string.Empty;

		public virtual string RecipientStreet { get; set; } = string.Empty;

		public virtual string RecipientHouseNumber { get; set; } = string.Empty;

		public virtual string RecipientPostalCode { get; set; } = string.Empty;

		public virtual string RecipientCity { get; set; } = string.Empty;

		public virtual DateTime IssueDate { get; set; }

		public virtual DateTime SaleDate { get; set; }

		public virtual PaymentMethod PaymentMethod { get; set; }

		public virtual string? BankAccountNumber { get; set; }

		public virtual DateTime PaymentDeadline { get; set; }

		public virtual Currency Currency { get; set; }

		public virtual bool IsSplitPayment { get; set; }

		public virtual decimal NetAmount { get; set; }

		public virtual decimal GrossAmount { get; set; }

		public virtual ICollection<InvoicePosition> InvoicePositions { get; set; } = new List<InvoicePosition>();

		public virtual Guid InsUserId { get; set; }

		public virtual DateTime InsDate { get; set; }

		public virtual Guid UpdUserId { get; set; }

		public virtual DateTime UpdDate { get; set; }
	}
}
