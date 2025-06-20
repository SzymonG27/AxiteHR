namespace AxiteHR.Services.DocumentAPI.Models.Invoice
{
	public class InvoiceGeneratorLog
	{
		public virtual int Id { get; set; }
		public virtual int InvoiceId { get; set; }
		public virtual string FileName { get; set; } = string.Empty;
		public virtual string FileExtension { get; set; } = string.Empty;
		public virtual DateTime? GeneratedDate { get; set; }
	}
}
