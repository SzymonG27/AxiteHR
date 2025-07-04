﻿using AxiteHR.Integration.Cache.Redis;
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

			var isCompanyUserHasAnyPermission = await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && permissionIdList.Contains(x.CompanyPermissionId))
				.AnyAsync();

			await redisCacheService.SetObjectAsync(
				CompanyRedisKeys.IsCompanyUserHasAnyPermission(companyUserId, permissionIdListOrderedString),
				isCompanyUserHasAnyPermission ? "true" : "false",
				TimeSpan.FromMinutes(5));

			return isCompanyUserHasAnyPermission;
		}
	}
}
