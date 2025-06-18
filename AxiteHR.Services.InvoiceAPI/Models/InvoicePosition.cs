using AxiteHR.Services.InvoiceAPI.Models.Enums;

namespace AxiteHR.Services.InvoiceAPI.Models
{
	public class InvoicePosition
	{
		public virtual int Id { get; set; }

		public virtual int InvoiceId { get; set; }

		public virtual Invoice Invoice { get; set; } = new();

		public virtual string ProductName { get; set; } = string.Empty;

		public virtual Unit Unit { get; set; }

		public virtual decimal Quantity { get; set; }

		public virtual decimal NetPrice { get; set; }

		public virtual int VatRate { get; set; }

		public virtual decimal VatAmount { get; set; }

		public virtual decimal NetAmount { get; set; }

		public virtual decimal GrossAmount { get; set; }
	}
}
