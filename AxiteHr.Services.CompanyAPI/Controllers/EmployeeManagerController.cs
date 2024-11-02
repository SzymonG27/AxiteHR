using AxiteHR.GlobalizationResources;
using AxiteHR.Integration.GlobalClass.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Services.Employee;
using Microsoft.Extensions.Localization;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Helpers;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeManagerController(
		IEmployeeService employeeService,
		IStringLocalizer<SharedResources> sharedLocalizer) : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> CreateNewEmployee(
			[FromBody] NewEmployeeRequestDto newEmployeeRequestDto,
			[FromHeader(Name = HeaderNamesHelper.AcceptLanguage)] string acceptLanguage = "en")
		{
			var bearerToken = await HttpContext.GetTokenAsync(HeaderNamesHelper.AccessTokenContext);
			if (string.IsNullOrEmpty(bearerToken))
			{
				return Unauthorized(sharedLocalizer[SharedResourcesKeys.Global_MissingToken]);
			}

			var response = await employeeService.CreateNewEmployeeAsync(newEmployeeRequestDto, bearerToken, acceptLanguage);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
