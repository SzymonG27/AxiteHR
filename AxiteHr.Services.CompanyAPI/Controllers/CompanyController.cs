using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.Auth;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AxiteHr.Services.CompanyAPI.Controllers
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

		[HttpGet("[action]/{companyId}&{excludedUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IEnumerable<CompanyUserViewDto>> CompanyUserList(int companyId, Guid excludedUserId, [FromQuery]Pagination paginationInfo)
		{
			if (!Request.Headers.TryGetValue("Authorization", out var token))
			{
				return [];
			}
			var bearerToken = token.ToString().Replace("Bearer ", "");

			return await companyService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken);
		}
	}
}
