using AxiteHR.Services.CompanyAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyPermission.Impl
{
	public class CompanyPermissionService(AppDbContext dbContext) : ICompanyPermissionService
	{
		public async Task<bool> IsCompanyUserHasPermissionAsync(int companyUserId, int permissionId)
		{
			return await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && x.CompanyPermissionId == permissionId)
				.AnyAsync();
		}

		public async Task<bool> IsCompanyUserHasAnyPermissionAsync(int companyUserId, List<int> permissionIdList)
		{
			return await dbContext.CompanyUserPermissions
				.Where(x => x.CompanyUserId == companyUserId && permissionIdList.Contains(x.CompanyPermissionId))
				.AnyAsync();
		}
	}
}
