using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl
{
	public class CompanyRoleService(
		AppDbContext dbContext,
		ICompanyUserService companyUserService,
		ICompanyPermissionService companyPermissionService) : ICompanyRoleService
	{
		public async Task<IEnumerable<CompanyRoleListResponseDto>> GetListAsync(CompanyRoleListRequestDto requestDto, Pagination pagination)
		{
			if (pagination.ItemsPerPage <= 0)
			{
				pagination.ItemsPerPage = 10;
			}

			var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId);

			if (companyUserId == null)
			{
				return [];
			}

			var isUserCanSeeEntireList = await IsUserCanSeeEntireListAsync(companyUserId.Value);

			var query = dbContext.CompanyRoles
				.Join(dbContext.CompanyRoleCompanies,
					cr => cr.Id,
					crc => crc.CompanyRoleId,
					(cr, crc) => new { cr, crc })
				.GroupJoin(dbContext.CompanyUserRoles,
					x => x.crc.Id,
					cur => cur.CompanyRoleCompanyId,
					(x, cur) => new { x.cr, x.crc, cur })
				.SelectMany(
					x => x.cur.DefaultIfEmpty(),
					(x, companyUser) => new { x.cr, x.crc, CompanyUser = companyUser })
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId);

			if (!isUserCanSeeEntireList)
			{
				query = query.Where(x => x.CompanyUser != null && x.CompanyUser.Id == companyUserId);
			}

			return await query.GroupBy(x => new { x.cr.Id, x.cr.RoleName, x.crc.IsMain })
				.OrderBy(x => x.Key.Id)
				.Skip(pagination.Page * pagination.ItemsPerPage)
				.Take(pagination.ItemsPerPage)
				.Select(x => new CompanyRoleListResponseDto
				{
					CompanyRoleId = x.Key.Id,
					Name = x.Key.RoleName,
					IsMain = x.Key.IsMain,
					EmployeesCount = dbContext.CompanyUserRoles.Count(cu => cu.CompanyRoleCompanyId == x.Key.Id)
				})
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto)
		{
			var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId);

			if (companyUserId == null)
			{
				return 0;
			}

			var isUserCanSeeEntireList = await IsUserCanSeeEntireListAsync(companyUserId.Value);

			var query = dbContext.CompanyRoles
				.Join(dbContext.CompanyRoleCompanies,
					cr => cr.Id,
					crc => crc.CompanyRoleId,
					(cr, crc) => new { cr, crc })
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId);

			if (!isUserCanSeeEntireList)
			{
				return await query
					.Join(dbContext.CompanyUserRoles,
						x => x.crc.Id,
						cur => cur.CompanyRoleCompanyId,
						(x, cur) => new { x.cr, x.crc, cur })
					.Where(x => x.cur.Id == companyUserId)
					.CountAsync();

			}

			return await query.CountAsync();
		}

		private async Task<bool> IsUserCanSeeEntireListAsync(int companyUserId)
		{
			var permissionThatCanSeeEntireList = new List<int>
			{
				(int)PermissionDictionary.CompanyManager,
				(int)PermissionDictionary.CompanyRoleSeeEntireList
			};

			return await companyPermissionService.IsCompanyUserHasAnyPermissionAsync(companyUserId, permissionThatCanSeeEntireList);
		}
	}
}
