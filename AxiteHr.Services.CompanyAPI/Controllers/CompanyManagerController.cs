using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Services.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController(ICompanyCreatorService companyCreatorService) : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize]
		public IActionResult CreateNewCompany([FromBody] NewCompanyRequestDto newCompanyRequest)
		{
			var response = companyCreatorService.NewCompanyCreate(newCompanyRequest);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok();
		}
	}
}
