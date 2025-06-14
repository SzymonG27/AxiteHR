using AxiteHR.Integration.GlobalClass.Auth;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyUserController(ICompanyUserService companyUserService) : ControllerBase
	{
		//TODO CompanyPermissionsValidator like CompanyRole>ListAsync
		[HttpGet("[action]/{companyId}/{excludedUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<IEnumerable<CompanyUserDataDto>> List(int companyId, Guid excludedUserId, [FromQuery] Pagination paginationInfo)
		{
			var bearerToken = await HttpContext.GetTokenAsync(HeaderNamesHelper.AccessTokenContext);
			if (string.IsNullOrEmpty(bearerToken))
			{
				return [];
			}

			return await companyUserService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken);
		}

		[HttpGet("[action]/{companyId}/{userId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<int?> GetId(int companyId, Guid userId)
		{
			return await companyUserService.GetIdAsync(companyId, userId);
		}

		[HttpGet("[action]/{companyId}/{excludedUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<int> Count(int companyId, Guid excludedUserId)
		{
			return await companyUserService.GetCompanyUsersCountAsync(companyId, excludedUserId);
		}

		[HttpGet("[action]/{userId}/{companyId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<bool> IsInCompany(Guid userId, int companyId)
		{
			return await companyUserService.IsUserInCompanyAsync(userId, companyId);
		}

		[HttpGet("[action]/{companyUserId}&{insUserId}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<bool> CanManageApplications(int companyUserId, Guid insUserId)
		{
			return await companyUserService.IsUserCanManageApplicationsAsync(companyUserId, insUserId);
		}
	}
}
