using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyRepository
	{
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);

		Task<IList<CompanyUserUserRelation>> GetCompanyUserUserRealtionListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo);

		Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId);

		/// <summary>
		/// Get company for employee. Employee should have one company.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns><see cref="CompanyForEmployeeDto"/></returns>
		Task<CompanyForEmployeeDto> GetCompanyForEmployeeDto(Guid employeeId);
	}
}
