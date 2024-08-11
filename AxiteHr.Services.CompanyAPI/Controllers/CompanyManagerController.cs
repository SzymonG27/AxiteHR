using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Models.Auth;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHr.Services.CompanyAPI.Services.Employee;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController(
		ICompanyCreatorService companyCreatorService,
		IEmployeeService employeeService,
		IStringLocalizer<SharedResources> sharedLocalizer) : ControllerBase
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

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> CreateNewEmployee([FromBody] NewEmployeeRequestDto newEmployeeRequestDto)
		{
			var isLanguageHeaderExists = Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage);
			if (!isLanguageHeaderExists)
			{
				acceptLanguage = "en";
			}
			if (!Request.Headers.TryGetValue("Authorization", out var token))
			{
				return Unauthorized(sharedLocalizer[SharedResourcesKeys.Global_MissingToken]);
			}
			var bearerToken = token.ToString().Replace("Bearer ", "");

			var response = await employeeService.CreateNewEmployeeAsync(newEmployeeRequestDto, bearerToken, acceptLanguage.ToString());
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
