using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission
{
	public interface ICompanyPermissionGroupService
	{
		Task<Result<int>> CreateGroupAsync(CreatePermissionGroupDto dto, Guid userId);
	}
}
