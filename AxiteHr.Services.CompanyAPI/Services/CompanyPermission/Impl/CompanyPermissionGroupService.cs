using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Models.Permissions;
using AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Net;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission.Impl
{
	public class CompanyPermissionGroupService(
		AppDbContext dbContext,
		ICompanyPermissionCheckService companyPermissionCheckService,
		ICompanyUserService companyUserService,
		IStringLocalizer<CompanyResources> companyLocalizer) : ICompanyPermissionGroupService
	{
		public async Task<Result<int>> CreateGroupAsync(CreatePermissionGroupDto dto, Guid userId)
		{
			var companyExists = await dbContext.Companies.AnyAsync(x => x.Id == dto.CompanyId);
			if (!companyExists)
			{
				Log.Error("User tried to create a permission within company, that doesn't exists, companyId: {CompanyId} | userId: {UserId}", dto.CompanyId, userId);
				return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_CreateGroupAsync_CompanyNotFound], HttpStatusCode.NotFound);
			}

			var companyUserId = await companyUserService.GetIdAsync(dto.CompanyId, userId);
			if (companyUserId == null)
			{
				return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_CreateGroupAsync_CompanyUserNotFound], HttpStatusCode.BadGateway);
			}

			if (!await companyPermissionCheckService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyPermissionManagePermissions))
			{
				return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_CreateGroupAsync_NoPermissionsToManage], HttpStatusCode.Unauthorized);
			}

			var nameExists = await dbContext.CompanyPermissionGroups.AnyAsync(x => x.CompanyId == dto.CompanyId && x.Name == dto.Name);
			if (nameExists)
			{
				var errorMessage = string.Format(companyLocalizer[CompanyResources.Permissions_CreateGroupAsync_PermissionNotExists], dto.Name);
				return Result<int>.Failure(errorMessage, HttpStatusCode.BadRequest);
			}

			if (dto.PermissionIds.Count != 0)
			{
				var validPermissionIds = await dbContext.CompanyPermissions
					.Where(x => dto.PermissionIds.Contains(x.Id))
					.Select(x => x.Id)
					.ToListAsync();

				var invalidIds = dto.PermissionIds.Except(validPermissionIds).ToList();
				if (invalidIds.Count != 0)
				{
					return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_CreateGroupAsync_PermissionNotExists], HttpStatusCode.NotFound);
				}
			}

			var group = new CompanyPermissionGroup
			{
				CompanyId = dto.CompanyId,
				Name = dto.Name,
				Description = dto.Description,
				IsActive = true,
				InsUserId = userId,
				InsDate = DateTime.UtcNow
			};

			await dbContext.CompanyPermissionGroups.AddAsync(group);
			await dbContext.SaveChangesAsync();

			if (dto.PermissionIds.Any())
			{
				var groupPermissions = dto.PermissionIds.ConvertAll(permissionId => new CompanyPermissionGroupPermission
				{
					CompanyPermissionGroupId = group.Id,
					CompanyPermissionId = permissionId,
					InsUserId = userId,
					InsDate = DateTime.UtcNow
				});

				await dbContext.CompanyPermissionGroupPermissions.AddRangeAsync(groupPermissions);
				await dbContext.SaveChangesAsync();
			}

			return Result<int>.Success(group.Id);
		}
	}
}
