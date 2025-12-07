using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Models.Permissions.Dto.Request;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission
{
	public interface ICompanyPermissionGroupService
	{
		Task<Result<int>> CreateGroupAsync(CreatePermissionGroupDto dto, Guid userId);

		Task<Result<bool>> UpdateGroupAsync(UpdatePermissionGroupDto dto, Guid userId);

		Task<Result<bool>> ChangeGroupActivityAsync(int groupId, Guid userId);

		Task<Result<bool>> PatchGroupPermissionsAsync(int groupId, List<int> permissionIds, Guid userId);

		Task<Result<bool>> DeleteGroupAsync(int groupId, Guid userId);
	}
}
