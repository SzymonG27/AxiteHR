using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHR.Services.CompanyAPI.Services.Company
{
	public interface ICompanyRepository
	{
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);

		/// <summary>
		/// Get company for employee. Employee should have one company.
		/// </summary>
		/// <param name="employeeId"></param>
		/// <returns><see cref="CompanyForEmployeeDto"/></returns>
		Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId);

		Task<int> GetCompanyUserIdAsync(Guid userId, int companyId);
	}
}
