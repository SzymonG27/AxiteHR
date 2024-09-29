using AxiteHr.Integration.GlobalClass.Auth;
using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

		[HttpGet("[action]/{companyId}/{excludedUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IEnumerable<CompanyUserViewDto>> CompanyUserList(int companyId, Guid excludedUserId, [FromQuery] Pagination paginationInfo)
		{
			var bearerToken = await HttpContext.GetTokenAsync(HeaderNamesHelper.AccessTokenContext);
			if (string.IsNullOrEmpty(bearerToken))
			{
				return [];
			}

			return await companyService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken);
		}

		[HttpGet("[action]/{companyId}/{excludedUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<int> CompanyUsersCount(int companyId, Guid excludedUserId)
		{
			return await companyService.GetCompanyUsersCountAsync(companyId, excludedUserId);
		}

		[HttpGet("[action]/{employeeId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.UserFromCompany)]
		public async Task<CompanyForEmployeeDto> GetForEmployee(Guid employeeId)
		{
			return await companyService.GetCompanyForEmployeeDtoAsync(employeeId);
		}

		[HttpGet("[action]/{userId}/{companyId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<bool> IsUserInCompany(Guid userId, int companyId)
		{
			return await companyService.IsUserInCompanyAsync(userId, companyId);
		}

		[HttpGet("[action]/{companyUserId}&{insUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<bool> IsUserCanManageApplications(int companyUserId, Guid insUserId)
		{
			return await companyService.IsUserCanManageApplicationsAsync(companyUserId, insUserId);
		}
	}
}
