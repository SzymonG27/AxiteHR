using AxiteHR.Services.InvoiceAPI.Models.Enums;

namespace AxiteHR.Services.InvoiceAPI.Models
{
	public class InvoiceSequence
	{
		public virtual int Id { get; set; }
		public virtual InvoiceType Type { get; set; }
		public virtual int CompanyUserId { get; set; }
		public virtual int Year { get; set; }
		public virtual int Month { get; set; }
		public virtual int CurrentNumber { get; set; }
	}
}
