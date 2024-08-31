using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using Microsoft.EntityFrameworkCore;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyRepository(AppDbContext dbContext) : ICompanyRepository
	{
		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			return [.. dbContext.CompanyUsers
				.Where(cu => cu.UserId == userId)
				.Join(dbContext.CompanyUserPermissions,
					cu => cu.Id,
					cup => cup.CompanyUserId,
					(cu, cup) => new { cu, cup })
				.Where(x => x.cup.CompanyPermissionId == (int)PermissionDictionary.CompanyManager)
				.Join(dbContext.Companies,
					x => x.cu.CompanyId,
					c => c.Id,
					(x, c) => new { c, x.cu })
				.Select(cu => new CompanyListDto
				{
					Id = cu.c.Id,
					CompanyName = cu.c.CompanyName,
					InsDate = cu.c.InsDate.ToShortDateString(),
					UserCount = dbContext.CompanyUsers.Count(u => u.CompanyId == cu.c.Id)
				})
				.AsNoTracking()];
		}

		public async Task<IList<CompanyUserUserRelation>> GetCompanyUserUserRealtionListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo)
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

		public async Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId)
		{
			return await dbContext.CompanyUsers
				.Where(x => x.CompanyId == companyId && x.UserId != excludedUserId)
				.AsNoTracking()
				.CountAsync();
		}

		public async Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId)
		{
			return await dbContext.CompanyUsers
				.AsNoTracking()
				.Where(cu => cu.UserId == employeeId)
				.Select(cu => new CompanyForEmployeeDto
				{
					CompanyId = cu.Company.Id,
					CompanyName = cu.Company.CompanyName
				})
				.SingleAsync();
		}

		public async Task<bool> IsUserInCompanyAsync(Guid userId, int companyId)
		{
			return await dbContext.CompanyUsers
				.AsNoTracking()
				.AnyAsync(cu => cu.UserId == userId && cu.CompanyId == companyId);
		}
	}
}
