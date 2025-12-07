namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission
{
	public interface ICompanyPermissionCheckService
	{
		Task<bool> IsCompanyUserHasPermissionAsync(int companyUserId, int permissionId);

		Task<bool> IsCompanyUserHasAnyPermissionAsync(int companyUserId, List<int> permissionIdList);

		Task<List<int>> GetAllUserPermissionIdsAsync(int companyUserId);
	}
}
