using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;

namespace AxiteHR.Services.CompanyAPI.Helpers
{
	public static class CompanyPermissionsHelper
	{
		public static readonly int[] ManagerPermissions =
		[
			(int)PermissionDictionary.CompanyManager
		];
	}
}
