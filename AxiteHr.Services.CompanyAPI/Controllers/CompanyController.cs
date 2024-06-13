using AxiteHr.Services.CompanyAPI.Models.Auth;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHr.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private readonly ICompanyService _companyService;

		public CompanyController(ICompanyService companyService)
		{
			_companyService = companyService;
		}

		[HttpGet("[action]/{userId}")]
		[Authorize]
		public IEnumerable<CompanyListDto> List(Guid userId)
		{
			var isTokenInHeader = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);
			return _companyService.GetCompanyList(userId);
		}
	}
}
