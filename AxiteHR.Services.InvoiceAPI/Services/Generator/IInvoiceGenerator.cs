using AxiteHR.Services.InvoiceAPI.Models.Dto.Generator;

namespace AxiteHR.Services.InvoiceAPI.Services.Generator
{
	public interface IInvoiceGenerator
	{
		Task<InvoiceGeneratorResponseDto> GenerateInvoiceAsync(InvoiceGeneratorRequestDto requestDto);
	}
}
