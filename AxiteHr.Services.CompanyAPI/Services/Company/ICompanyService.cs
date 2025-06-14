using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHR.Services.CompanyAPI.Services.Company
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
		/// Get company for employee. Employee should have one company.
		/// </summary>
		/// <param name="employeeId">Employee identifier</param>
		/// <returns><see cref="CompanyForEmployeeDto"/></returns>
		Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId);

		Task<int> GetCompanyUserIdAsync(Guid userId, int companyId);
	}
}
