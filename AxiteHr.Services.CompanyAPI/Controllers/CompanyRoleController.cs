using AxiteHR.Integration.GlobalClass.Auth;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyRoleController(ICompanyRoleService companyRoleService) : ControllerBase
	{
		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public IEnumerable<CompanyRoleListResponseDto> List([FromQuery] CompanyRoleListRequestDto requestDto, [FromQuery] Pagination pagination)
		{
			return companyRoleService.GetList(requestDto, pagination);
		}

		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.Admin},{Roles.User}")]
		public async Task<int> CountAsync([FromQuery] CompanyRoleListRequestDto requestDto)
		{
			return await companyRoleService.GetCountListAsync(requestDto);
		}
	}
}
