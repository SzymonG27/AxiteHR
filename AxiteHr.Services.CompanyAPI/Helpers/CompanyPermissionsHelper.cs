using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;

namespace AxiteHR.Services.CompanyAPI.Helpers
{
	public static class CompanyPermissionsHelper
	{
		public static readonly List<int> ManagerPermissions =
		[
			(int)PermissionDictionary.CompanyManager
		];

		#region Company Role
		public static readonly List<int> CompanyRoleSeeEntireListPermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleSeeEntireList
		];

		public static readonly List<int> CompanyRoleCreatePermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleCreator
		];

		public static readonly List<int> CompanyRoleAttachUserPermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleCreator
		];
		#endregion
	}
}
