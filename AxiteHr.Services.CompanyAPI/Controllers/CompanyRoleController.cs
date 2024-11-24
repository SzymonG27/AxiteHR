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
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IEnumerable<CompanyRoleListResponseDto>> ListAsync([FromQuery] CompanyRoleListRequestDto requestDto, [FromQuery] Pagination pagination)
		{
			return await companyRoleService.GetListAsync(requestDto, pagination);
		}

		[HttpGet("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<int> CountAsync([FromQuery] CompanyRoleListRequestDto requestDto)
		{
			return await companyRoleService.GetCountListAsync(requestDto);
		}

		[HttpPost("[action]")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> CreateAsync([FromBody] CompanyRoleCreatorRequestDto requestDto)
		{
			var response = await companyRoleService.CreateAsync(requestDto);
			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
	}
}
