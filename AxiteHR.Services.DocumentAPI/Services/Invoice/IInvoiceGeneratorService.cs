using AxiteHR.Services.DocumentAPI.Models.Invoice.Dto;

namespace AxiteHR.Services.DocumentAPI.Services.Invoice
{
	public interface IInvoiceGeneratorService
	{
		Task<string> GenerateInvoiceAsync(InvoiceGeneratorDto model);
	}
}
