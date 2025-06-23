namespace AxiteHR.Services.DocumentAPI.Models.Invoice.Dto
{
	public record InvoiceGeneratorDto
	{
		public string BlobFileName { get; set; } = string.Empty;

		public string LogoBase64 { get; set; } = string.Empty;
	}
}
