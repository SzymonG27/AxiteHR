﻿using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Integration.GlobalClass.Redis.Keys;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Infrastructure.AuthApi;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;
using Microsoft.EntityFrameworkCore;
using CompanyUserModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyUser;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyUser.Impl
{
	public class CompanyUserService(
		AppDbContext dbContext,
		IRedisCacheService redisCacheService,
		IAuthApiClient authApiClient) : ICompanyUserService
	{
		public async Task<IEnumerable<CompanyUserDataDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken)
		{
			if (paginationInfo.ItemsPerPage <= 0)
			{
				paginationInfo.ItemsPerPage = 10;
			}

			var companyUserIds = await GetCompanyUserUserRealtionListAsync(companyId, excludedUserId, paginationInfo);

			if (companyUserIds.Count == 0)
			{
				return [];
			}

			return await authApiClient.GetUserDataListDtoAsync(companyUserIds, bearerToken);
		}

		public async Task<int?> GetIdAsync(int companyId, Guid userId)
		{
			var companyUserIdFromCache = await redisCacheService.GetObjectAsync<int>(CompanyRedisKeys.CompanyUserGetId(companyId, userId));
			if (companyUserIdFromCache is not default(int))
			{
				return companyUserIdFromCache;
			}

			var companyUserId = await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId == userId)
				.AsNoTracking()
				.Select(x => x.Id)
				.SingleOrDefaultAsync();

			if (companyUserId is default(int))
			{
				return null;
			}

			await redisCacheService.SetObjectAsync(CompanyRedisKeys.CompanyUserGetId(companyId, userId), companyUserId, TimeSpan.FromMinutes(5));

			return companyUserId;
		}

		public async Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId)
		{
			return await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId != excludedUserId)
				.AsNoTracking()
				.CountAsync();
		}

		public async Task<bool> IsUserInCompanyAsync(Guid userId, int companyId)
		{
			return await dbContext.CompanyUsers
				.AsNoTracking()
				.AnyAsync(cu => cu.UserId == userId && cu.CompanyId == companyId);
		}

		public async Task<bool> IsUserCanManageApplicationsAsync(int companyUserId, Guid insUserId)
		{
			var companyUser = await GetCompanyUserAsync(companyUserId);
			if (companyUser == null)
			{
				return false;
			}

			//CompanyUser created application is the same as ins user, return true
			if (companyUser.UserId == insUserId)
			{
				return true;
			}

			//Else validate supervisor
			var insUserRole = await GetCompanyUserMainRoleAsync(companyUser.CompanyId, insUserId);
			if (insUserRole == null)
			{
				return false;
			}

			if (!insUserRole.IsSupervisor)
			{
				return false;
			}

			var companyUserRole = await GetCompanyUserMainRoleAsync(companyUserId);
			if (companyUserRole == null)
			{
				return false;
			}

			//InsUser is supervisor and main roles are the same, return true, else false
			return companyUserRole.CompanyRoleCompanyId == insUserRole.CompanyRoleCompanyId;
		}

		#region Private Methods
		private async Task<IList<CompanyUserUserRelation>> GetCompanyUserUserRealtionListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo)
		{
			return await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId != excludedUserId)
				.OrderBy(x => x.Id)
				.Skip(paginationInfo.Page * paginationInfo.ItemsPerPage)
				.Take(paginationInfo.ItemsPerPage)
				.AsNoTracking()
				.Select(x => new CompanyUserUserRelation { CompanyUserId = x.Id, UserId = x.UserId })
				.ToListAsync();
		}

		private async Task<CompanyUserModel?> GetCompanyUserAsync(int companyUserId)
		{
			return await dbContext.CompanyUsers
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Id == companyUserId);
		}

		private async Task<CompanyUserRole?> GetCompanyUserMainRoleAsync(int companyUserId)
		{
			return await dbContext.CompanyUserRoles
				.AsNoTracking()
				.Join(dbContext.CompanyRoleCompanies,
					cur => cur.CompanyRoleCompanyId,
					crc => crc.Id,
					(cur, crc) => new { cur, crc.IsMain })
				.Where(x => x.cur.CompanyUserId == companyUserId && x.IsMain)
				.Select(x => x.cur)
				.SingleOrDefaultAsync();
		}

		private async Task<CompanyUserRole?> GetCompanyUserMainRoleAsync(int companyId, Guid insUserId)
		{
			return await dbContext.CompanyUserRoles
				.AsNoTracking()
				.Join(dbContext.CompanyRoleCompanies,
					cur => cur.CompanyRoleCompanyId,
					crc => crc.Id,
					(cur, crc) => new { cur, crc.IsMain, crc.CompanyId })
				.Where(x => x.IsMain && x.CompanyId == companyId)
				.Join(dbContext.CompanyUsers,
					x => x.cur.CompanyUserId,
					cu => cu.Id,
					(x, cu) => new { x.cur, cu })
				.Where(x => x.cu.UserId == insUserId)
				.Select(x => x.cur)
				.SingleOrDefaultAsync();
		}
		#endregion Private Methods
	}
}
