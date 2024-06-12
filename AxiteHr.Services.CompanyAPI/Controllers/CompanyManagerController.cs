using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize]
		public IActionResult CreateNewCompany([FromBody] NewCompanyRequestDto newCompanyRequest)
		{
			return Ok("Working perfect");
		}
	}
}
