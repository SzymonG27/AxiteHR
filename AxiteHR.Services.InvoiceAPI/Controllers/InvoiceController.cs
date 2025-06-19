using AxiteHR.Services.InvoiceAPI.Models.Dto.Generator;
using AxiteHR.Services.InvoiceAPI.Services.Generator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.InvoiceAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InvoiceController(IInvoiceGenerator invoiceGenerator) : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType(typeof(InvoiceGeneratorResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(InvoiceGeneratorResponseDto), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GenerateAsync([FromBody] InvoiceGeneratorRequestDto requestDto)
		{
			var responseDto = await invoiceGenerator.GenerateInvoiceAsync(requestDto);

			if (!responseDto.IsSucceeded)
			{
				return BadRequest(responseDto);
			}

			return Ok(responseDto);
		}
	}
}
