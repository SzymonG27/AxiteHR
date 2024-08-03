using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyCreatorService
	{
		/// <summary>
		/// Create new company request
		/// </summary>
		/// <param name="newCompanyRequest">DTO of new company data</param>
		/// <returns>NewCompanyReponseDto</returns>
		Task<NewCompanyReponseDto> NewCompanyCreateAsync(NewCompanyRequestDto newCompanyRequest);
	}
}