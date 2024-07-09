using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyCreatorService
	{
		Task<NewCompanyReponseDto> NewCompanyCreate(NewCompanyRequestDto newCompanyRequest);
	}
}