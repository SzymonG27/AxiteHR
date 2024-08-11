using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyRepository
	{
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);

		Task<IList<CompanyUserUserRelation>> GetCompanyUserUserRealtionListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo);

		Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId);
	}
}
