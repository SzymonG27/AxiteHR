using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Services.Company.Impl
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
	}
}
