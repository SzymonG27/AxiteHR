using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Models.Auth;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHr.Services.CompanyAPI.Services.Employee;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyManagerController(
		ICompanyCreatorService companyCreatorService,
		IEmployeeService employeeService) : ControllerBase
	{
		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IActionResult> CreateNewCompany([FromBody] NewCompanyRequestDto newCompanyRequest)
		{
			var response = await companyCreatorService.NewCompanyCreate(newCompanyRequest);
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
			var response = await employeeService.CreateNewEmployee(newEmployeeRequestDto);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}
	}
}
