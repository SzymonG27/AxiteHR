using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyCreatorService(AppDbContext dbContext) : ICompanyCreatorService
	{
		public NewCompanyReponseDto NewCompanyCreate(NewCompanyRequestDto newCompanyRequest)
		{
			throw new NotImplementedException();
		}
	}
}