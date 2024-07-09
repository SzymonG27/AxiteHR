using AxiteHr.Services.CompanyAPI.Models.Auth;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController(ICompanyService companyService) : ControllerBase
	{
		[HttpGet("[action]/{userId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public IEnumerable<CompanyListDto> List(Guid userId)
		{
			return companyService.GetCompanyList(userId);
		}
	}
}
