using AxiteHR.Services.CompanyAPI.Models.Permissions.Const;

namespace AxiteHR.Services.CompanyAPI.Helpers
{
	public static class CompanyPermissionsHelper
	{
		public static readonly List<int> UserManagerPermissions =
		[
			(int)PermissionDictionary.CompanyManager,
		];

		#region Company Role
		public static readonly List<int> CompanyRoleSeeEntireListPermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleManager,
			(int)PermissionDictionary.CompanyRoleSeeEntireList
		];

		public static readonly List<int> CompanyRoleCreatePermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleManager,
			(int)PermissionDictionary.CompanyRoleCreator
		];

		public static readonly List<int> CompanyRoleAttachUserPermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyRoleCreator
		];
		#endregion

		#region Company permission
		public static readonly List<int> CompanyPermissionManagePermissions = [
			(int)PermissionDictionary.CompanyManager,
			(int)PermissionDictionary.CompanyPermissionManager
		];
		#endregion
	}
}
