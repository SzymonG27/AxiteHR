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
		/// <param name="employeeId">Employee identifier</param>
		/// <returns><see cref="CompanyForEmployeeDto"/></returns>
		Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId);

		/// <summary>
		/// Determines whether a specified user is associated with a given company.
		/// </summary>
		/// <param name="userId">The unique identifier of the user to check.</param>
		/// <param name="companyId">The unique identifier of the company to check against.</param>
		/// <returns>
		/// A boolean value wrapped in an observable indicating whether the user is a member of the company:
		/// <list type="bullet">
		/// <item>
		/// <description><c>true</c> - The user is a member of the specified company.</description>
		/// </item>
		/// <item>
		/// <description><c>false</c> - The user is not a member of the specified company.</description>
		/// </item>
		/// </list>
		/// </returns>
		Task<bool> IsUserInCompanyAsync(Guid userId, int companyId);
	}
}
