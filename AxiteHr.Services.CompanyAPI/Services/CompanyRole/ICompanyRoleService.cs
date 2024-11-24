using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole
{
	public interface ICompanyRoleService
	{
		IEnumerable<CompanyRoleListResponseDto> GetList(CompanyRoleListRequestDto requestDto, Pagination pagination);

		Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto);
	}
}
