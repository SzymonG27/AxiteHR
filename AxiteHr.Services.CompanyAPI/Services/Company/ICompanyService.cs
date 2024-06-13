using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyService
	{
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);
	}
}
