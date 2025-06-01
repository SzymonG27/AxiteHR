using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyRoleController(ICompanyRoleService companyRoleService) : ControllerBase
	{
		[HttpGet("ListAsync")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IEnumerable<CompanyRoleListResponseDto>> ListAsync([FromQuery] CompanyRoleListRequestDto requestDto, [FromQuery] Pagination pagination)
		{
			return await companyRoleService.GetListAsync(requestDto, pagination);
		}

		[HttpGet("CountAsync")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<int> CountAsync([FromQuery] CompanyRoleListRequestDto requestDto)
		{
			return await companyRoleService.GetCountListAsync(requestDto);
		}

		[HttpPost("CreateAsync")]
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

		[HttpPost("ListEmployeesToAttachAsync")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IEnumerable<CompanyUserDataDto>> ListEmployeesToAttachAsync([FromBody] CompanyRoleUserToAttachRequestDto requestDto, [FromQuery] Pagination pagination)
		{
			var bearerToken = await HttpContext.GetTokenAsync(HeaderNamesHelper.AccessTokenContext);
			if (string.IsNullOrEmpty(bearerToken))
			{
				return [];
			}

			return await companyRoleService.GetListOfEmployeesToAttachAsync(requestDto, pagination, bearerToken);
		}

		[HttpPost("AttachUserAsync")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<IActionResult> AttachUserAsync([FromBody] CompanyRoleAttachUserRequestDto requestDto)
		{
			var response = await companyRoleService.AttachUserAsync(requestDto);

			if (!response.IsSucceeded)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
	}
}
