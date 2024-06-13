using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyService : ICompanyService
	{
		private readonly AppDbContext _dbContext;
		public CompanyService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			return _dbContext.CompanyUsers
				.Join(_dbContext.CompanyUserPermissions,
					o => o.Id,
					i => i.CompanyUserId,
					(o, i) => new
					{
						CompanyUserId = o.Id,
						o.UserId,
						o.CompanyId,
						i.CompanyPermissionId
					})
				.Join(_dbContext.Companies,
					o => o.CompanyId,
					i => i.Id,
					(o, i) => new
					{
						o.CompanyUserId,
						o.UserId,
						o.CompanyId,
						o.CompanyPermissionId,
						i.CompanyName,
						i.InsDate
					})
				.Where(x => x.UserId == userId && x.CompanyPermissionId == PermissionDictionary.CompanyManager)
				.Select(x => new CompanyListDto
				{
					Id = x.CompanyId,
					CompanyName = x.CompanyName,
					InsDate = x.InsDate.ToShortDateString()
				})
				.ToList();
		}
	}
}