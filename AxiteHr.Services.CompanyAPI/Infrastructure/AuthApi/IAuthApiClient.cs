using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;

namespace AxiteHR.Services.CompanyAPI.Infrastructure.AuthApi
{
	public interface IAuthApiClient
	{
		Task<IEnumerable<CompanyUserDataDto>> GetUserDataListDtoAsync(IList<CompanyUserUserRelation> companyUserRelations, string bearerToken);
	}
}
