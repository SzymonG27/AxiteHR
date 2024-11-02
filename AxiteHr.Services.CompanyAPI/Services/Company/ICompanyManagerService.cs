using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHR.Services.CompanyAPI.Services.Company
{
	public interface ICompanyManagerService
	{
		/// <summary>
		/// Create new company request
		/// </summary>
		/// <param name="newCompanyRequest">DTO of new company data</param>
		/// <returns>NewCompanyReponseDto</returns>
		Task<NewCompanyReponseDto> NewCompanyCreateAsync(NewCompanyRequestDto newCompanyRequest);
	}
}