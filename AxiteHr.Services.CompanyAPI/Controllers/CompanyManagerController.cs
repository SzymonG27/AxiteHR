using AxiteHR.Integration.GlobalClass.Auth;
using AxiteHR.Services.CompanyAPI.Services.Company;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController(ICompanyManagerService companyCreatorService) : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> CreateNewCompany([FromBody] NewCompanyRequestDto newCompanyRequest)
		{
			var response = await companyCreatorService.NewCompanyCreateAsync(newCompanyRequest);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok();
		}
	}
}
