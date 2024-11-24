using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole
{
	public interface ICompanyRoleService
	{
		Task<IEnumerable<CompanyRoleListResponseDto>> GetListAsync(CompanyRoleListRequestDto requestDto, Pagination pagination);

		Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto);
	}
}
