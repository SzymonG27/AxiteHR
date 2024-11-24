namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission
{
	public interface ICompanyPermissionService
	{
		Task<bool> IsCompanyUserHasPermissionAsync(int companyUserId, int permissionId);

		Task<bool> IsCompanyUserHasAnyPermissionAsync(int companyUserId, List<int> permissionIdList);
	}
}
