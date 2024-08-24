using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyService
	{
		/// <summary>
		/// Get simple enumerable dto of company
		/// </summary>
		/// <param name="userId">User identifier for company search</param>
		/// <returns>IEnumerable of CompanyListDto</returns>
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);

		/// <summary>
		/// Get enumerable dto of employees contains in company, exclude user who is searching
		/// </summary>
		/// <param name="companyId">Company identifier</param>
		/// <param name="excludedUserId">User identifier to exclude from list</param>
		/// <param name="paginationInfo">Pagination info</param>
		/// <param name="bearerToken">Token for auth data request</param>
		/// <returns>IEnumerable of CompanyUserViewDto</returns>
		Task<IEnumerable<CompanyUserViewDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken);

		/// <summary>
		/// Get count of employees contains in company, exclude user who is searching
		/// </summary>
		/// <param name="companyId">Company identifier</param>
		/// <param name="excludedUserId">User identifier to exclude from count</param>
		/// <returns>Company users count</returns>
		Task<int> GetCompanyUsersCountAsync(int companyId, Guid excludedUserId);

		/// <summary>
		/// Get company for employee. Employee should have one company.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns><see cref="CompanyForEmployeeDto"/></returns>
		Task<CompanyForEmployeeDto> GetCompanyForEmployeeDto(Guid employeeId);
	}
}
