using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;

namespace AxiteHr.Services.CompanyAPI.Services.Company
{
	public interface ICompanyService
	{
		IEnumerable<CompanyListDto> GetCompanyList(Guid userId);

		Task<IEnumerable<CompanyUserViewDto>> GetCompanyUserViewDtoListAsync(int companyId, Guid excludedUserId, Pagination paginationInfo, string bearerToken);
	}
}
