using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
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
		IStringLocalizer<CompanyResources> companyLocalizer,
		IRedisCacheService redisCacheService) : ICompanyPermissionGroupService
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
				return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_CompanyUserNotFound], HttpStatusCode.BadGateway);
			}

			if (!await companyPermissionCheckService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyPermissionManagePermissions))
			{
				return Result<int>.Failure(companyLocalizer[CompanyResources.Permissions_NoPermissionsToManage], HttpStatusCode.Unauthorized);
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

			if (dto.PermissionIds.Count != 0)
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

		public async Task<Result<bool>> UpdateGroupAsync(UpdatePermissionGroupDto dto, Guid userId)
		{
			var group = await dbContext.CompanyPermissionGroups.FindAsync(dto.GroupId);

			if (group == null)
			{
				return Result<bool>.Failure("Permission group not found", HttpStatusCode.NotFound);
			}

			if (group.Name != dto.Name)
			{
				var nameExists = await dbContext.CompanyPermissionGroups
					.AnyAsync(x => x.CompanyId == group.CompanyId && x.Name == dto.Name && x.Id != dto.GroupId);

				if (nameExists)
				{
					return Result<bool>.Failure($"Permission group with name '{dto.Name}' already exists in this company", HttpStatusCode.BadRequest);
				}
			}

			group.Name = dto.Name;
			group.Description = dto.Description;
			group.IsActive = dto.IsActive;
			group.UpdUserId = userId;
			group.UpdDate = DateTime.UtcNow;

			await dbContext.SaveChangesAsync();

			await InvalidateGroupUsersCacheAsync(dto.GroupId);

			return Result<bool>.Success(true);
		}

		public async Task<Result<bool>> ChangeGroupActivityAsync(int groupId, Guid userId)
		{
			var group = await dbContext.CompanyPermissionGroups.FindAsync(groupId);

			if (group == null)
			{
				return Result<bool>.Failure("Permission group not found", HttpStatusCode.NotFound);
			}

			var companyUserId = await companyUserService.GetIdAsync(group.CompanyId, userId);
			if (companyUserId == null)
			{
				return Result<bool>.Failure(companyLocalizer[CompanyResources.Permissions_CompanyUserNotFound], HttpStatusCode.BadGateway);
			}

			if (!await companyPermissionCheckService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyPermissionManagePermissions))
			{
				return Result<bool>.Failure(companyLocalizer[CompanyResources.Permissions_NoPermissionsToManage], HttpStatusCode.Unauthorized);
			}

			group.IsActive = !group.IsActive;
			group.UpdUserId = userId;
			group.UpdDate = DateTime.UtcNow;

			await dbContext.SaveChangesAsync();
			await InvalidateGroupUsersCacheAsync(groupId);

			return Result<bool>.Success(true);
		}

		public async Task<Result<bool>> PatchGroupPermissionsAsync(int groupId, List<int> permissionIds, Guid userId)
		{
			var group = await dbContext.CompanyPermissionGroups.FindAsync(groupId);
			if (group == null)
			{
				return Result<bool>.Failure("Permission group not found", HttpStatusCode.NotFound);
			}

			if (permissionIds.Count != 0)
			{
				var validPermissionIds = await dbContext.CompanyPermissions
					.Where(x => permissionIds.Contains(x.Id))
					.Select(x => x.Id)
					.ToListAsync();

				var invalidIds = permissionIds.Except(validPermissionIds).ToList();

				if (invalidIds.Count != 0)
				{
					return Result<bool>.Failure($"Invalid permission IDs: {string.Join(", ", invalidIds)}", HttpStatusCode.BadRequest);
				}
			}

			var existingPermissions = await dbContext.CompanyPermissionGroupPermissions
				.Where(x => x.CompanyPermissionGroupId == groupId)
				.ToListAsync();

			var existingPermissionIds = existingPermissions.Select(x => x.CompanyPermissionId).ToHashSet();

			var permissionsToAdd = permissionIds
				.Where(id => !existingPermissionIds.Contains(id))
				.Select(id => new CompanyPermissionGroupPermission
				{
					CompanyPermissionGroupId = groupId,
					CompanyPermissionId = id,
					InsUserId = userId,
					InsDate = DateTime.UtcNow
				})
				.ToList();

			if (permissionsToAdd.Count != 0)
			{
				await dbContext.CompanyPermissionGroupPermissions.AddRangeAsync(permissionsToAdd);
			}

			var permissionsToRemove = existingPermissions
				.Where(x => !permissionIds.Contains(x.CompanyPermissionId))
				.ToList();

			if (permissionsToRemove.Count != 0)
			{
				dbContext.CompanyPermissionGroupPermissions.RemoveRange(permissionsToRemove);
			}

			group.UpdUserId = userId;
			group.UpdDate = DateTime.UtcNow;

			await dbContext.SaveChangesAsync();
			await InvalidateGroupUsersCacheAsync(groupId);

			return Result<bool>.Success(true);
		}

		public async Task<Result<bool>> DeleteGroupAsync(int groupId, Guid userId)
		{
			var group = await dbContext.CompanyPermissionGroups
				.Include(x => x.UserPermissions)
				.FirstOrDefaultAsync(x => x.Id == groupId);

			if (group == null)
			{
				return Result<bool>.Failure("Permission group not found", HttpStatusCode.NotFound);
			}

			if (group.UserPermissions.Count != 0)
			{
				return Result<bool>.Failure($"Cannot delete group. It is assigned to {group.UserPermissions.Count} user(s). Please remove all assignments first or deactivate the group instead.", HttpStatusCode.BadRequest);
			}

			var companyUserId = await companyUserService.GetIdAsync(group.CompanyId, userId);
			if (companyUserId == null)
			{
				return Result<bool>.Failure(companyLocalizer[CompanyResources.Permissions_CompanyUserNotFound], HttpStatusCode.BadGateway);
			}

			if (!await companyPermissionCheckService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyPermissionManagePermissions))
			{
				return Result<bool>.Failure(companyLocalizer[CompanyResources.Permissions_DeleteGroupAsync_NoPermissionsToDelete], HttpStatusCode.Unauthorized);
			}

			dbContext.CompanyPermissionGroups.Remove(group);
			await dbContext.SaveChangesAsync();
			await InvalidateGroupUsersCacheAsync(groupId);

			return Result<bool>.Success(true);
		}

		private async Task InvalidateGroupUsersCacheAsync(int groupId)
		{
			var companyUserIds = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyPermissionGroupId == groupId)
				.Select(x => x.CompanyUserId)
				.Distinct()
				.ToListAsync();

			foreach (var companyUserId in companyUserIds)
			{
				await redisCacheService.DeleteAsync(CompanyRedisKeys.AllUserPermissions(companyUserId));

				await redisCacheService.DeleteByPatternAsync(CompanyRedisKeys.IsCompanyUserHasPermissionDeletePattern(companyUserId));

				await redisCacheService.DeleteByPatternAsync(CompanyRedisKeys.IsCompanyUserHasAnyPermissionDeletePattern(companyUserId));
			}
		}
	}
}
