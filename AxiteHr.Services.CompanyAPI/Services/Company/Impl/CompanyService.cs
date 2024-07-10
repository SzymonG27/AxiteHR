using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using Microsoft.EntityFrameworkCore;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyService(AppDbContext dbContext) : ICompanyService
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
	}
}