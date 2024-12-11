using AxiteHR.Integration.GlobalClass.Auth;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Services.Company;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController(
		ICompanyService companyService) : ControllerBase
	{
		[HttpGet("[action]/{userId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public IEnumerable<CompanyListDto> List(Guid userId)
		{
			return companyService.GetCompanyList(userId);
		}

		[HttpGet("[action]/{userId}/{companyId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<int> CompanyUserId(Guid userId, int companyId)
		{
			return await companyService.GetCompanyUserIdAsync(userId, companyId);
		}

		[HttpGet("[action]/{employeeId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.UserFromCompany)]
		public async Task<CompanyForEmployeeDto> GetForEmployee(Guid employeeId)
		{
			return await companyService.GetCompanyForEmployeeDtoAsync(employeeId);
		}
	}
}
