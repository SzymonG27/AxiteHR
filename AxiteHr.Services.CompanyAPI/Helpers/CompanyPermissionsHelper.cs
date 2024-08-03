using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;

namespace AxiteHr.Services.CompanyAPI.Helpers
{
	public static class CompanyPermissionsHelper
	{
		public static readonly int[] ManagerPermissions =
		[
			(int)PermissionDictionary.CompanyManager
		];
	}
}
