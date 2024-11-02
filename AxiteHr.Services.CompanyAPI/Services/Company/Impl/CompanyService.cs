using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHR.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
	{
		public IEnumerable<CompanyListDto> GetCompanyList(Guid userId)
		{
			return companyRepository.GetCompanyList(userId);
		}

		public async Task<CompanyForEmployeeDto> GetCompanyForEmployeeDtoAsync(Guid employeeId)
		{
			return await companyRepository.GetCompanyForEmployeeDtoAsync(employeeId);
		}
	}
}