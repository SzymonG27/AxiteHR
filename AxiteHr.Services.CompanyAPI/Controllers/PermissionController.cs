using AxiteHR.Services.CompanyAPI.Extensions;
using AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Response;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AxiteHR.Services.CompanyAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PermissionController(
		ICompanyUserService companyUserService,
		ICompanyPermissionCheckService companyPermissionCheckService,
		ICompanyPermissionGroupService companyPermissionGroupService) : ControllerBase
	{
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("user/permissions")]
		public async Task<ActionResult<UserPermissionsResponseDto>> GetCurrentUserPermissionsAsync([FromQuery] int companyId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var companyUserId = await companyUserService.GetIdAsync(companyId, Guid.Parse(userId));

			if (companyUserId == null)
			{
				return Ok(new UserPermissionsResponseDto
				{
					CompanyId = companyId,
					UserId = Guid.Parse(userId)
				});
			}

			var permissions = await companyPermissionCheckService.GetAllUserPermissionIdsAsync(companyUserId.Value);

			return Ok(new UserPermissionsResponseDto
			{
				CompanyUserId = companyUserId,
				CompanyId = companyId,
				UserId = Guid.Parse(userId),
				Permissions = permissions
			});
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("group/create")]
		public async Task<IActionResult> CreateGroupAsync([FromBody] CreatePermissionGroupDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var result = await companyPermissionGroupService.CreateGroupAsync(dto, Guid.Parse(userId));

			if (!result.IsSuccess)
			{
				return this.Error(result.StatusCode, result.Error);
			}

			return Ok(result.Value);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut("group/update")]
		public async Task<IActionResult> UpdateGroupAsync([FromBody] UpdatePermissionGroupDto dto)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var result = await companyPermissionGroupService.UpdateGroupAsync(dto, Guid.Parse(userId));

			if (!result.IsSuccess)
			{
				return this.Error(result.StatusCode, result.Error);
			}

			return Ok(result.Value);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPatch("group/{groupId}/permissions/update")]
		public async Task<IActionResult> UpdateGroupPermissionsAsync(int groupId, [FromBody] List<int> permissionIds)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var result = await companyPermissionGroupService.PatchGroupPermissionsAsync(groupId, permissionIds, Guid.Parse(userId));

			if (!result.IsSuccess)
			{
				return this.Error(result.StatusCode, result.Error);
			}

			return Ok(result.Value);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPatch("group/changeactivity")]
		public async Task<IActionResult> ChangeGroupActivityAsync([FromBody] int groupId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var result = await companyPermissionGroupService.ChangeGroupActivityAsync(groupId, Guid.Parse(userId));

			if (!result.IsSuccess)
			{
				return this.Error(result.StatusCode, result.Error);
			}

			return Ok(result.Value);
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpDelete("group/delete")]
		public async Task<IActionResult> DeleteGroupAsync([FromBody] int groupId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			var result = await companyPermissionGroupService.DeleteGroupAsync(groupId, Guid.Parse(userId));

			if (!result.IsSuccess)
			{
				return this.Error(result.StatusCode, result.Error);
			}

			return Ok(result.Value);
		}
	}
}
