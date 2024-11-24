using AxiteHR.Integration.GlobalClass.RedisKeys;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Services.Cache;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission.Impl
{
	public class CompanyPermissionService(
		AppDbContext dbContext,
		IRedisCacheService redisCacheService) : ICompanyPermissionService
	{
		public async Task<bool> IsCompanyUserHasPermissionAsync(int companyUserId, int permissionId)
		{
			var valueFromRedis = await redisCacheService.GetObjectAsync<bool?>(CompanyRedisKeys.IsCompanyUserHasPermission(companyUserId, permissionId));

			if (valueFromRedis is not null)
			{
				return valueFromRedis.Value;
			}

			var isCompanyUserHasPermission = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && x.CompanyPermissionId == permissionId)
				.AnyAsync();

			await redisCacheService.SetObjectAsync(
				CompanyRedisKeys.IsCompanyUserHasPermission(companyUserId, permissionId),
				isCompanyUserHasPermission,
				TimeSpan.FromMinutes(5));

			return isCompanyUserHasPermission;
		}

		public async Task<bool> IsCompanyUserHasAnyPermissionAsync(int companyUserId, List<int> permissionIdList)
		{
			var permissionIdListOrderedString = string.Join(",", permissionIdList.Order().ToList());

			var valueFromRedis = await redisCacheService.GetObjectAsync<bool?>(CompanyRedisKeys.IsCompanyUserHasAnyPermission(companyUserId, permissionIdListOrderedString));

			if (valueFromRedis is not null)
			{
				return valueFromRedis.Value;
			}

			var isCompanyUserHasAnyPermission = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && permissionIdList.Contains(x.CompanyPermissionId))
				.AnyAsync();

			await redisCacheService.SetObjectAsync(
				CompanyRedisKeys.IsCompanyUserHasAnyPermission(companyUserId, permissionIdListOrderedString),
				isCompanyUserHasAnyPermission,
				TimeSpan.FromMinutes(5));

			return isCompanyUserHasAnyPermission;
		}
	}
}
