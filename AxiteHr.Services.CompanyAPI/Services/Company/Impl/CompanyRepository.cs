using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using Microsoft.EntityFrameworkCore;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyRepository(AppDbContext dbContext) : ICompanyRepository
	{
		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			var companyUsersCount = dbContext.CompanyUsers
				.GroupBy(cu => cu.CompanyId)
				.AsNoTracking()
				.Select(g => new
				{
					CompanyId = g.Key,
					UserCount = g.Count()
				});

			return [.. dbContext.CompanyUsers
				.Join(dbContext.CompanyUserPermissions,
					cu => cu.Id,
					cup => cup.CompanyUserId,
					(cu, cup) => new { cu, cup })
				.Where(x => x.cu.UserId == userId && x.cup.CompanyPermissionId == (int)PermissionDictionary.CompanyManager)
				.Join(dbContext.Companies,
					previous => previous.cu.CompanyId,
					c => c.Id,
					(previous, c) => new { previous.cu, previous.cup, c })
				.Join(companyUsersCount,
					previous => previous.cu.CompanyId,
					count => count.CompanyId,
					(previous, count) => new
					{
						CompanyId = previous.c.Id,
						previous.c.CompanyName,
						previous.c.InsDate,
						count.UserCount
					})
				.AsNoTracking()
				.Select(x => new CompanyListDto
				{
					Id = x.CompanyId,
					CompanyName = x.CompanyName,
					InsDate = x.InsDate.ToShortDateString(),
					UserCount = x.UserCount
				})];
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
	}
}
