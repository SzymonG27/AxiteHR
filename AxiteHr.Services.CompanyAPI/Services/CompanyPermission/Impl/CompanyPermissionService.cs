using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
using AxiteHR.Services.CompanyAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission.Impl
{
	public class CompanyPermissionService(
		AppDbContext dbContext,
		IRedisCacheService redisCacheService) : ICompanyPermissionService
	{
		public async Task<bool> IsCompanyUserHasPermissionAsync(int companyUserId, int permissionId)
		{
			var valueFromRedis = await redisCacheService.GetObjectAsync<string?>(CompanyRedisKeys.IsCompanyUserHasPermission(companyUserId, permissionId));

			if (valueFromRedis is not null)
			{
				return bool.Parse(valueFromRedis);
			}

			var isCompanyUserHasPermission = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && x.CompanyPermissionId == permissionId)
				.AnyAsync();

			await redisCacheService.SetObjectAsync(
				CompanyRedisKeys.IsCompanyUserHasPermission(companyUserId, permissionId),
				isCompanyUserHasPermission? "true" : "false",
				TimeSpan.FromMinutes(5));

			return isCompanyUserHasPermission;
		}

		public async Task<bool> IsCompanyUserHasAnyPermissionAsync(int companyUserId, List<int> permissionIdList)
		{
			var permissionIdListOrderedString = string.Join(",", permissionIdList.Order().ToList());

			var valueFromRedis = await redisCacheService.GetObjectAsync<string?>(CompanyRedisKeys.IsCompanyUserHasAnyPermission(companyUserId, permissionIdListOrderedString));

			if (valueFromRedis is not null)
			{
				return bool.Parse(valueFromRedis);
			}

			var hasPermission = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId)
				.Where(x =>
					(x.CompanyPermissionId.HasValue && permissionIdList.Contains(x.CompanyPermissionId.Value))
					||
					(x.CompanyPermissionGroupId.HasValue
					 && x.CompanyPermissionGroup!.IsActive
					 && x.CompanyPermissionGroup.Permissions.Any(p => permissionIdList.Contains(p.CompanyPermissionId)))
				)
				.AnyAsync();

			await redisCacheService.SetObjectAsync(
				CompanyRedisKeys.IsCompanyUserHasAnyPermission(companyUserId, permissionIdListOrderedString),
				hasPermission ? "true" : "false",
				TimeSpan.FromMinutes(5));

			return hasPermission;
		}

		public async Task<List<int>> GetAllUserPermissionIdsAsync(int companyUserId)
		{
			var cacheKey = CompanyRedisKeys.AllUserPermissions(companyUserId);
			var valueFromRedis = await redisCacheService.GetObjectAsync<List<int>?>(cacheKey);

			if (valueFromRedis is not null)
			{
				return valueFromRedis;
			}

			var permissions = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId)
				.Select(x => new
				{
					DirectPermission = x.CompanyPermissionId,
					GroupPermissions = x.CompanyPermissionGroupId.HasValue && x.CompanyPermissionGroup!.IsActive
						? x.CompanyPermissionGroup.Permissions.Select(p => p.CompanyPermissionId).ToList()
						: new List<int>()
				})
				.ToListAsync();

			var allPermissions = permissions
				.SelectMany(x => x.DirectPermission.HasValue
					? x.GroupPermissions.Append(x.DirectPermission.Value)
					: x.GroupPermissions)
				.Distinct()
				.ToList();

			await redisCacheService.SetObjectAsync(cacheKey, allPermissions, TimeSpan.FromMinutes(5));

			return allPermissions;
		}
	}
}
